using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance { get; private set; }

    public float Score { get; private set; }

    [SerializeField]
    private int bastScore;

    // �÷��̾ ������ ��ġ
    [SerializeField]
    private Transform playerTransform;

    private Player player;

    [SerializeField]
    private GameObject changeCharacters;

    private bool isGameOver = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        MakePlayer();
        
    }

    private void Start()
    {
        Score = 0;
        // Player Ŭ������ �����ͼ� �÷��̾ player�� �Ҵ�

        player = FindObjectOfType<Player>();

        // bastScore �ʱ�ȭ
        int dungeonIndex = DataManager.Instance.currentDungeon;
        var bastScores = DataManager.Instance.currentPlayerdata.bastScores;

        while (bastScores.Count <= dungeonIndex)
        {
            if (bastScores != null && dungeonIndex < bastScores.Count)
            {
                bastScore = bastScores[dungeonIndex];
                break;
            }
            else
            {
                DataManager.Instance.currentPlayerdata.bastScores.Add(0);
            }
        }
        ChangeCharacterImage();

        //UI ���� �ʱ�ȭ
        UIManager_InGame.Instance.myScore = (int)Score;
        UIManager_InGame.Instance.highScore = bastScore;
        UIManager_InGame.Instance.UpdateHighScoreText();
        UIManager_InGame.Instance.UpdateMyScoreText();
        changeCharacters = GameObject.Find("ChangeCharacters");


        // �� �̸� ������Ʈ
        UIManager_InGame.Instance.mapNameText = dungeonIndex + " ��������";
        UIManager_InGame.Instance.UpdateMapNameText();
    }

    private void Update()
    {
        if (player.isDead)
        {
            if (isGameOver)
                return;
            Debug.Log("Player is dead");
            Daed();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerStat playerstat = player.GetComponent<PlayerStat>();
            SetSpeed(playerstat.moveSpeed + 2);
        }

        float amount = 1;

        if (player.isMoveCheck)
        {
            IncreaseScore(amount);
            //IncreaseProgressSliderUIBar();
        }
    }

    // �÷��̾� ���� �Լ�
    private void MakePlayer()
    {
        string characterName = DataManager.Instance.currentPlayerdata.currentCharacter.ToString();
        GameObject playerPrefab = Resources.Load<GameObject>("Prefab/Player/" + characterName);
        Instantiate(playerPrefab, playerTransform);
    }


    // playerstat.moveSpeed 30 ����
    public void SetSpeed(float newSpeed)
    {
        // �÷��̾��� PlayerStat ������Ʈ�� ������
        PlayerStat playerstat = player.GetComponent<PlayerStat>();
        Player playerP = player.GetComponent<Player>();
        // 1. ���� �ӵ� ������Ʈ
        playerstat.moveSpeed = newSpeed;

        if (playerstat.moveSpeed > 30)
        {
            playerstat.moveSpeed = 30; // �ִ� �ӵ� ����
        }

        // 2. ���� �ӵ����� ���� ��� (0���� ������ ���� ����)
        float speedRatio = 1f;
        if (playerstat.baseMoveSpeed > 0)
        {
            speedRatio = playerstat.moveSpeed / playerstat.baseMoveSpeed;
        }

        // 3. ������ ���� ���� ���� �߷� ������ ����
        playerstat.jumpForce = playerstat.baseJumpForce * speedRatio;
        playerstat.currentGravityScale = playerstat.baseGravityScale * Mathf.Pow(speedRatio, 2);

        // 4. ���� �߷� ���� Rigidbody�� ��� ����
        playerP._rigidbody.gravityScale = playerstat.currentGravityScale;
    }

    public void Daed()
    {
        isGameOver = true;

        DataManager.Instance.currentPlayerdata.bastScores[DataManager.Instance.currentDungeon] = bastScore;
        UIManager_InGame.Instance.ShowResultUI(); // StopButton

    }

    // ���ھ� ����
    void IncreaseScore(float amount)
    {
        Score += amount;
        UIManager_InGame.Instance.myScore = (int)Score;
        UIManager_InGame.Instance.UpdateMyScoreText(); 
        if (Score >= bastScore)
        {
            bastScore = (int)Score;
            UIManager_InGame.Instance.highScore = bastScore;
            UIManager_InGame.Instance.UpdateHighScoreText();
        } 
    }

    void IncreaseProgressSliderUIBar()
    {
        //Debug.Log($"player.transform.position.x : {player.transform.position.x + 8.5f}");
        //Debug.Log($"�̰ź��� Ŀ���� : {MapManager.Instance.totalMapLength * MapManager.Instance.loopPoint - 8.5f}");
        //Debug.Log($"�̰ź��� �۾ƾ��� : {MapManager.Instance.totalMapLength * MapManager.Instance.loopPoint + MapManager.Instance.fixWidth -8.5f}");
        //Debug.Log($"MapManager.Instance.loopPoint : {player.transform.position.x + 8.5f >= (((MapManager.Instance.loopPoint) * MapManager.Instance.totalMapLength) + MapManager.Instance.fixWidth - 8.5f)}");

        if (MapManager.Instance.totalMapLength * MapManager.Instance.loopPoint - 8.5f <= player.transform.position.x + 8.5f &&
            MapManager.Instance.totalMapLength * MapManager.Instance.loopPoint + MapManager.Instance.fixWidth - 8.5f  >= player.transform.position.x + 8.5f) return;
  
        float playerPosX = player.transform.position.x + 8.5f + (MapManager.Instance.loopPoint * MapManager.Instance.fixWidth);
        float a = playerPosX % (MapManager.Instance.totalMapLength-8.5f);

        //Debug.Log($"playerPosX : {a}");
        a =  a / (MapManager.Instance.totalMapLength-8.5f);
        //Debug.Log($"totalMapLength : {a}");


        UIManager_InGame.Instance.UpdateProgressSlider(a);
    }

    public void ChangeCharacterImage()
    {
        // �ڽ� ������Ʈ ������ 
        Transform[] gameObjects = changeCharacters.GetComponentsInChildren<Transform>(true);


        foreach (Transform t in gameObjects)
        {
            // �θ� ������Ʈ�� �ǳʶ�
            if (t == changeCharacters.transform)
                continue;

            // ĳ���� �̸��� ���� �÷��̾� �������� ĳ���� �̸� ��
            if (t.name == DataManager.Instance.currentPlayerdata.currentCharacter.ToString())
            {
                t.gameObject.SetActive(true);
            }
            else
            {
                t.gameObject.SetActive(false);
            }
        }
    }
}
