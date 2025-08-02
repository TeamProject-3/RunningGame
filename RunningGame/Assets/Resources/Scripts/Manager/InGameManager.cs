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

    private MapManager mapManager;

    private bool isGameOver = false;

    private float progressBarNum = 0f;


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
        changeCharacters = GameObject.Find("ChangeCharacters");
    }

    private void Start()
    {
        Score = 0;
        // Player Ŭ������ �����ͼ� �÷��̾ player�� �Ҵ�

        player = FindObjectOfType<Player>();
        mapManager = FindObjectOfType<MapManager>();

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


        //UI ���� �ʱ�ȭ
        UIManager_InGame.Instance.myScore = (int)Score;
        UIManager_InGame.Instance.highScore = bastScore;
        UIManager_InGame.Instance.UpdateHighScoreText();
        UIManager_InGame.Instance.UpdateMyScoreText();



        // �� �̸� ������Ʈ
        UIManager_InGame.Instance.mapNameText = dungeonIndex + " ��������";
        UIManager_InGame.Instance.UpdateMapNameText();
    }

    private void Update()
    {
        if (player.isDead)
        {
            Debug.Log("Player is dead");
            IsDaed();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerStat playerstat = player.GetComponent<PlayerStat>();
            SetSpeed(playerstat.moveSpeed + 2);
        }

        float amount = 1;

        //�� ó�� �÷��̾� �浹�� ���� �� ���α׷����� ����
        if (player.isMoveCheck)
        {
            IncreaseScore(amount);
            IncreaseBar();
        }
    }

    // �÷��̾� ���� �Լ�
    private void MakePlayer()
    {
        string characterName = DataManager.Instance.currentPlayerdata.currentCharacter.ToString();
        GameObject playerPrefab = Resources.Load<GameObject>("Prefab/Player/" + characterName);
        Instantiate(playerPrefab, playerTransform);
    }

    public void SetSpeed(float newSpeed)
    {
        // �÷��̾��� PlayerStat ������Ʈ�� ������
        PlayerStat playerstat = player.GetComponent<PlayerStat>();
        Player playerP = player.GetComponent<Player>();
        // 1. ���� �ӵ� ������Ʈ
        playerstat.moveSpeed = newSpeed;

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
        // playerP._rigidbody.gravityScale = playerstat.currentGravityScale;
    }

    public void IsDaed()
    {
        if (!isGameOver)
            return;
        isGameOver = true;
        // �÷��̾ �׾��� �� ȣ��Ǵ� �Լ�
        // ���� ���, ���� ���� UI�� ǥ���ϰų� ����ŸƮ�ϴ� ������ ���⿡ �߰��� �� �ֽ��ϴ�.
        DataManager.Instance.currentPlayerdata.bastScores[DataManager.Instance.currentDungeon] = bastScore;
        UIManager_InGame.Instance.ShowResultUI(); // StopButton
    }

    // ���ھ� ����
    void IncreaseScore(float amount)
    {
        Score += amount;
        UIManager_InGame.Instance.myScore = (int)Score;
        UIManager_InGame.Instance.UpdateMyScoreText();
        if (Score > bastScore)
        {
            UIManager_InGame.Instance.UpdateHighScoreText();
            UIManager_InGame.Instance.highScore = bastScore;
            bastScore = (int)Score;
        }
    }

    void IncreaseBar()
    {

        //if (mapManager.ProgressMapCheck()) return;
        float playerX = player.transform.position.x + 8.5f - (mapManager.loopPoint * 54f);
        //float playerX = player.transform.position.x + 8.5f;
        float loopX = playerX % (mapManager.totalMapLength - 18f);

        if (loopX < 0)
            loopX += mapManager.totalMapLength + 9.5f;

        progressBarNum = Mathf.Clamp01(loopX  / (mapManager.totalMapLength - 18f) );

        UIManager_InGame.Instance.UpdateProgressSlider(progressBarNum);

       // Debug.Log(progressBarNum);

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
