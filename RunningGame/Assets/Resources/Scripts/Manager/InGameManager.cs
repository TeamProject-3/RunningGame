using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance { get; private set; }

    public int Score { get; private set; }

    // 플레이어가 생성될 위치
    [SerializeField]
    private Transform playerTransform;

    private Player player;
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

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            PlayerStat playerstat = player.GetComponent<PlayerStat>();
            SetSpeed(playerstat.moveSpeed + 2);
        }
    }
    public void AddScore(int amount)
    {
        Score += amount;
        Debug.Log("Score added: " + amount + ". Total score: " + Score);
    }

    // 플레이어 생성 함수
    private void MakePlayer()
    {
        string characterName = DataManager.Instance.currentPlayerdata.currentCharacter.ToString();
        GameObject playerPrefab = Resources.Load<GameObject>("Prefab/Player/" + characterName);
        Debug.Log("Player Prefab: " + playerPrefab?.name + characterName);
        Instantiate(playerPrefab, playerTransform);
    }


    public void SetSpeed(float newSpeed)
    {
        // 플레이어의 PlayerStat 컴포넌트를 가져옴
        PlayerStat playerstat = player.GetComponent<PlayerStat>();
        Player playerP = player.GetComponent<Player>();
        // 1. 현재 속도 업데이트
        playerstat.moveSpeed = newSpeed;

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
        // playerP._rigidbody.gravityScale = playerstat.currentGravityScale;
    }
}
