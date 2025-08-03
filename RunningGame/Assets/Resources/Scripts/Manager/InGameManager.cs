using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance { get; private set; }

    public float Score { get; private set; }

    [SerializeField]
    private int bastScore;

    
    public int coinCount;
    // 플레이어가 생성될 위치
    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private int MaxSpeed = 15;

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
        // Player 클래스를 가져와서 플레이어를 player에 할당

        player = FindObjectOfType<Player>();

        // bastScore 초기화
        int dungeonIndex = DataManager.Instance.currentDungeon;
        var bastScores = DataManager.Instance.currentPlayerdata.bastScores;

        while (bastScores.Count <= dungeonIndex)
        {
            if (bastScores != null && dungeonIndex < bastScores.Count)
            {
                
                Debug.Log($"Bast Score for dungeon {dungeonIndex}: {bastScore}");
                break;
            }
            else
            {
                DataManager.Instance.currentPlayerdata.bastScores.Add(0);
            }
        } 
        bastScore = bastScores[dungeonIndex];
        ChangeCharacterImage();

        //UI 점수 초기화
        UIManager_InGame.Instance.myScore = (int)Score;
        UIManager_InGame.Instance.highScore = bastScore;
        UIManager_InGame.Instance.UpdateHighScoreText();
        UIManager_InGame.Instance.UpdateMyScoreText();

        // 코인 초기화
        UIManager_InGame.Instance.coinCount = DataManager.Instance.currentPlayerdata.gold;
        UIManager_InGame.Instance.UpdateCoinText();

        changeCharacters = GameObject.Find("ChangeCharacters");


        // 맵 이름 업데이트
        UIManager_InGame.Instance.mapNameText = dungeonIndex + " 스테이지";
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
            //IncreaseScore(amount);
            IncreaseProgressSliderUIBar();
        }
    }

    // 플레이어 생성 함수
    private void MakePlayer()
    {
        string characterName = DataManager.Instance.currentPlayerdata.currentCharacter.ToString();
        GameObject playerPrefab = Resources.Load<GameObject>("Prefab/Player/" + characterName);
        Instantiate(playerPrefab, playerTransform);
    }

    public void SetSpeed(float newSpeed)
    {
        // 플레이어의 PlayerStat 컴포넌트를 가져옴
        PlayerStat playerstat = player.GetComponent<PlayerStat>();
        Player playerP = player.GetComponent<Player>();
        // 1. 현재 속도 업데이트
        playerstat.moveSpeed = newSpeed;

        if (playerstat.moveSpeed > MaxSpeed)
        {
            playerstat.moveSpeed = MaxSpeed; // 최대 속도 제한
        }

        // 2. 기준 속도와의 비율 계산 (0으로 나누는 오류 방지)
        float speedRatio = 1f;
        if (playerstat.baseMoveSpeed > 0)
        {
            speedRatio = playerstat.moveSpeed / playerstat.baseMoveSpeed;
        }

        // 3. 비율에 맞춰 점프 힘과 중력 스케일 재계산
        playerstat.jumpForce = playerstat.baseJumpForce * speedRatio;
        playerstat.currentGravityScale = playerstat.baseGravityScale * Mathf.Pow(speedRatio, 2);

        // 4. 계산된 중력 값을 Rigidbody에 즉시 적용
        playerP._rigidbody.gravityScale = playerstat.currentGravityScale;
    }

    public void Daed()
    {
        isGameOver = true;

        DataManager.Instance.currentPlayerdata.bastScores[DataManager.Instance.currentDungeon] = bastScore;
        UIManager_InGame.Instance.ExitScoreText();
        UIManager_InGame.Instance.ShowResultUI(); // StopButton
        DataManager.Instance.currentPlayerdata.gold += coinCount;
        
        DataManager.Instance.OnSaveData();

    }

    // 스코어 증가
    public void IncreaseScore(float amount)
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
        float playerPosX = player.transform.position.x + 8.5f + (MapManager.Instance.loopPoint * MapManager.Instance.fixWidth);
        //float a = playerPosX % (MapManager.Instance.totalMapLength-8.5f);
        float a = playerPosX / (MapManager.Instance.totalMapLength + MapManager.Instance.fixWidth);
        a = a - (int)(playerPosX / (MapManager.Instance.totalMapLength + MapManager.Instance.fixWidth));
        Debug.Log($"playerPosX : {playerPosX}");
        Debug.Log($"MapManager.Instance.totalMapLength : {MapManager.Instance.totalMapLength}");


        UIManager_InGame.Instance.UpdateProgressSlider(a);
    }

    public void ChangeCharacterImage()
    {
        // 자식 오브젝트 가져옴 
        Transform[] gameObjects = changeCharacters.GetComponentsInChildren<Transform>(true);


        foreach (Transform t in gameObjects)
        {
            // 부모 오브젝트는 건너뜀
            if (t == changeCharacters.transform)
                continue;

            // 캐릭터 이름과 현재 플레이어 데이터의 캐릭터 이름 비교
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
