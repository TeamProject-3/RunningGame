using System.Collections;
using UnityEngine;

public enum CharacterType
{
    PlayerFishy,
    PlayerOrcy,
    PlayerPescy,
    PlayerSharky
}

public class Player : MonoBehaviour
{
    Animator animator;
    public Rigidbody2D _rigidbody;
    SpriteRenderer spriteRenderer;
    PlayerStat playerstat;
    Transform _transform;

    [SerializeField] private Collider2D mainCollier;
    [SerializeField] private Collider2D slidingCollider;
    [SerializeField] private Collider2D mainshield;
    [SerializeField] private Collider2D slidingshield;
    [SerializeField] private GameObject shieldObj;
    Coroutine shieldCoroutine; // 슬라이딩 중에 방패를 활성화하기 위한 코루틴

    public int jumpCount = 0;
    public int maxJumpCount = 2; // Maximum number of jumps allowed
    public float damage = 10; // Maximum number of jumps allowed
    public bool isDead = false;
    public bool isJump = false;
    public bool isSliding = false;
    public bool isMoveCheck = false; // MoveCheck 오브젝트와 충돌 여부
    public bool isShield = false;
    public bool isSBoost = false;




    public float hpDecreaseRate = 0.8f; //HP 감소속도



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerstat = GetComponent<PlayerStat>();
        _transform = GetComponent<Transform>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found in children of Player.");
        }

        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody2D component not found on Player.");
        }

        if (slidingCollider != null)
        {
            slidingCollider.enabled = false;
        }

        playerstat.maxHp = playerstat.Hp; // 플레이어의 최대 HP 설정
        DeActivateShield();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (!isMoveCheck)
        {
            // MoveCheck 오브젝트와 충돌 전에는 플레이어의 입력을 무시하고 HP 감소를 하지 않음
            return;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (isSliding)
                {
                    StopSliding();
                }

                if (jumpCount < maxJumpCount)
                {

                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0); // Y축 속도 초기화
                    _rigidbody.AddForce(Vector2.up * playerstat.jumpForce, ForceMode2D.Impulse);
                    jumpCount++;
                    animator.SetTrigger("Jump");
                    animator.SetBool("Jump", true);

                    isJump = true;
                }
            }
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetMouseButtonDown(2))
            {
                StartSliding();
            }
            else if (isSliding)
            {
                StopSliding();
            }
        }
        if (_transform.position.y < -6)
        {
            DeathTrigger(); // 일정 y축 이하로 떨어지면 죽음 처리
        }

        if (isDead) return;
        else
        {
            if (_rigidbody.velocity.y < -1)
            {
                animator.SetBool("Jump", true);
                Debug.DrawRay(_rigidbody.position, Vector2.down * 2, Color.green);
                RaycastHit2D rayhit = Physics2D.Raycast(_rigidbody.position, Vector2.down, 2, LayerMask.GetMask("Ground"));
                if (rayhit.collider != null)
                {

                    jumpCount = 0;
                    animator.SetBool("Jump", false);
                    //Debug.Log("Raycast hit: " + rayhit.collider.name);

                    isJump = false;
                }
            }
        }
        DecreaseHpOverTime();
    }


    private void FixedUpdate()
    {
        if (isDead) return;
        _rigidbody.velocity = new Vector2(playerstat.moveSpeed, _rigidbody.velocity.y);
    }
    // 슬라이딩 
    void StartSliding()
    {
        if (!isJump)
            if (!isSliding)
            {
                //  if (isShield)
                isSliding = true;
                mainCollier.enabled = false; // 메인 콜라이더 비활성화
                slidingCollider.enabled = true; // 슬라이딩 콜라이더 활성화
                animator.SetBool("isSliding", true);
                _rigidbody.velocity = new Vector2(playerstat.moveSpeed, _rigidbody.velocity.y); // X축 속도 설정
                UpdateShield();
            }
    }

    void StopSliding()
    {
        //if (!isJump)
        Debug.Log("슬라이딩 중지");
        if (isSliding)
        {
            isSliding = false;
            mainCollier.enabled = true; // 메인 콜라이더 활성화
            slidingCollider.enabled = false; // 슬라이딩 콜라이더 비활성화
            animator.SetBool("isSliding", false);
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y); // X축 속도 초기화
            UpdateShield();
        }
    }

    //온트리거 이벤트
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.tag == "Enemy")
        {
            if (isShield)
            {
                Invoke("DeActivateShield", 0.5f); // 충돌 이후 0.5초뒤에 보호막 해제
                Debug.Log("적과 충돌 실드해제됨");
                return; // 방패가 활성화되어 있으면 피해를 받지 않음
            }
            //Debug.Log("Player hit by enemy");
            OnDamaged(); //피격시 함수 호출

        }

        else if (collision.CompareTag("MoveCheck"))
        {
            // MoveCheck에 닿으면 동작 활성화
            isMoveCheck = true;
            Debug.Log("MoveCheck 충돌, 플레이어 조작 및 HP 감소 활성화!");
        }

    }

    public void ShieldSkill()
    {
        // Shield에 닿으면 방패 활성화
        if (shieldCoroutine != null) StopCoroutine(shieldCoroutine);
        shieldCoroutine = StartCoroutine(ShieldTimer(7f));
        shieldObj.SetActive(true); // 방패 오브젝트 활성화
        Debug.Log("실드 아이템 획득: 7초간 실드 활성화");
    }

    void OnDamaged()
    {
        gameObject.layer = 9; // "PlayerDamaged" 레이어로 변경

        spriteRenderer.color = new Color(1, 1, 1, 0.4f); // 피격시 투명하게 변경
        animator.SetTrigger("isOnhit"); // 피격 애니메이션 트리거
        playerstat.Hp -= damage; // HP 감소
        Invoke("OffDamaged", 1); //1초 동안 무적후에 호출
    }

    void OffDamaged()
    {

        gameObject.layer = 6; // "Player" 레이어로 변경
        spriteRenderer.color = new Color(1, 1, 1, 1); // 투명도 원래대로 변경
    }


    //HP가 시간이 지나면서 감소하는 함수
    void DecreaseHpOverTime()
    {
        if (playerstat.Hp > 0)
        {
            playerstat.Hp -= Time.deltaTime * hpDecreaseRate; // 초당 0.5씩 감소
            //Debug.Log("Player HP: " + playerstat.Hp);
        }
        else
        {
            isDead = true;
            animator.SetTrigger("isDie");
            _rigidbody.velocity = Vector2.zero; // 플레이어 정지
            Debug.Log("Player is dead");
        }
    }

    // 일정 y축 이하로 떨어지면 죽음 처리
    void DeathTrigger()
    {
        isDead = true;
        mainCollier.enabled = false; // 메인 콜라이더 비활성화
        slidingCollider.enabled = false; // 슬라이딩 콜라이더 비활성화
        animator.SetTrigger("isDie");
        _rigidbody.velocity = Vector2.zero; // 플레이어 정지
        _rigidbody.simulated = false;
    }

    void ActiveShield()
    {
        isShield = true;
        UpdateShield();
    }


    // 처음 실행할때 초기화
    void DeActivateShield()
    {
        isShield = false;
        if (mainshield != null)
        {
            mainshield.enabled = false; // 메인 방패 콜라이더 비활성화
        }
        if (slidingshield != null)
        {
            slidingshield.enabled = false; // 슬라이딩 방패 콜라이더 비활성화
        }
        shieldObj.SetActive(false);
    }

    void UpdateShield()
    {
        if (!isShield)
        {
            if (mainshield != null) mainshield.enabled = false;
            if (slidingshield != null) slidingshield.enabled = false;
            return;
        }
        if (isSliding)
        {
            if (mainshield != null) mainshield.enabled = false;
            if (slidingshield != null) slidingshield.enabled = true;
        }
        else
        {
            if (mainshield != null) mainshield.enabled = true;
            if (slidingshield != null) slidingshield.enabled = false;
        }
    }

    // HP 회복 함수
    public void Heal(float amount)
    {
        playerstat.Hp += amount;
        if (playerstat.Hp > playerstat.maxHp)
        {
            playerstat.Hp = playerstat.maxHp; // 최대 HP를 초과하지 않도록 제한
        }
        Debug.Log("Player healed: " + amount + ", Current HP: " + playerstat.Hp);
    }


    IEnumerator ShieldTimer(float duration)
    {
        ActiveShield(); // 방패 활성화
        yield return new WaitForSeconds(duration); // 지정된 시간 동안 대기
        DeActivateShield(); // 방패 비활성화
        Debug.Log("실드 아이템 효과 종료");
    }

    //private void OnEnable()
    //{
    //    isDead = false;
    //    jumpCount = 0;
    //   isSliding = false;
    //   if (slidingCollider != null)
    //    {
    //        slidingCollider.enabled = false;
    //    }
    //}

}
