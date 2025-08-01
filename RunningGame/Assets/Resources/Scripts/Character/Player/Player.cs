using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    Rigidbody2D _rigidbody;
    SpriteRenderer spriteRenderer;
    PlayerStat playerstat;
    Transform _transform;

    [SerializeField] private Collider2D mainCollier;
    [SerializeField] private Collider2D slidingCollider;
    [SerializeField] private Collider2D mainshield;
    [SerializeField] private Collider2D slidingshield;

   
    public int jumpCount = 0;
    public int maxJumpCount = 2; // Maximum number of jumps allowed
    public bool isDead = false;
    public bool isJump = false;
    public bool isSliding = false;
     
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

        if(slidingCollider != null)
        {
            slidingCollider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {


        if (isDead)
        {
            return;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
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
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetMouseButtonDown(2) && isJump)
            {
                StartSliding();
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetMouseButtonUp(2) && isJump)
            {
                StopSliding();
            }
        }
        if (_transform.position.y < -6)
        {
            DeathTrigger(); // 일정 y축 이하로 떨어지면 죽음 처리
        }

        _rigidbody.velocity = new Vector2(playerstat.moveSpeed, _rigidbody.velocity.y);
        DecreaseHpOverTime();
    }


    private void FixedUpdate()
    {
        if (isDead) return;
        else
        {
            if (_rigidbody.velocity.y < -1)
            {
                animator.SetBool("Jump", true);
                Debug.DrawRay(_rigidbody.position, Vector2.down, Color.green);
                RaycastHit2D rayhit = Physics2D.Raycast(_rigidbody.position, Vector2.down, 2, LayerMask.GetMask("Ground"));
                if (rayhit.collider != null)
                {
                    if (rayhit.distance < 1)
                    {
                        jumpCount = 0;
                        animator.SetBool("Jump", false);
                        //Debug.Log("Raycast hit: " + rayhit.collider.name);

                        isJump = false;
                    }
                }
            }
        }
    }
    // 슬라이딩 
    void StartSliding()
    {
        if(!isJump)
        if (!isSliding)
        {
            isSliding = true;
            mainCollier.enabled = false; // 메인 콜라이더 비활성화
            slidingCollider.enabled = true; // 슬라이딩 콜라이더 활성화
            animator.SetBool("isSliding", true);
            _rigidbody.velocity = new Vector2(playerstat.slideSpeed, _rigidbody.velocity.y); // X축 속도 설정

        }
    }

    void StopSliding()
    {
        if (!isJump)
         if (isSliding)
        {
            isSliding = false;
            mainCollier.enabled = true; // 메인 콜라이더 활성화
            slidingCollider.enabled = false; // 슬라이딩 콜라이더 비활성화
            animator.SetBool("isSliding", false);
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y); // X축 속도 초기화
        }
    }

    //피격시 무적 
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        else
        {
            if (collision.gameObject.tag == "Enemy")
            {
                Debug.Log("Player hit by enemy");
                OnDamaged(); //피격시 함수 호출

            }
        }
    }

    void OnDamaged()
    {
        gameObject.layer = 9; // "PlayerDamaged" 레이어로 변경

        spriteRenderer.color = new Color(1, 1, 1, 0.4f); // 피격시 투명하게 변경
        animator.SetTrigger("isOnhit"); // 피격 애니메이션 트리거
        playerstat.Hp -= 1; // HP 감소
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
