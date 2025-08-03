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
    Coroutine shieldCoroutine; // �����̵� �߿� ���и� Ȱ��ȭ�ϱ� ���� �ڷ�ƾ

    public int jumpCount = 0;
    public int maxJumpCount = 2; // Maximum number of jumps allowed
    public float damage = 10; // Maximum number of jumps allowed
    public bool isDead = false;
    public bool isJump = false;
    public bool isSliding = false;
    public bool isMoveCheck = false; // MoveCheck ������Ʈ�� �浹 ����
    public bool isShield = false;
    public bool isSBoost = false;




    public float hpDecreaseRate = 0.8f; //HP ���Ҽӵ�



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

        playerstat.maxHp = playerstat.Hp; // �÷��̾��� �ִ� HP ����
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
            // MoveCheck ������Ʈ�� �浹 ������ �÷��̾��� �Է��� �����ϰ� HP ���Ҹ� ���� ����
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

                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0); // Y�� �ӵ� �ʱ�ȭ
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
            DeathTrigger(); // ���� y�� ���Ϸ� �������� ���� ó��
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
    // �����̵� 
    void StartSliding()
    {
        if (!isJump)
            if (!isSliding)
            {
                //  if (isShield)
                isSliding = true;
                mainCollier.enabled = false; // ���� �ݶ��̴� ��Ȱ��ȭ
                slidingCollider.enabled = true; // �����̵� �ݶ��̴� Ȱ��ȭ
                animator.SetBool("isSliding", true);
                _rigidbody.velocity = new Vector2(playerstat.moveSpeed, _rigidbody.velocity.y); // X�� �ӵ� ����
                UpdateShield();
            }
    }

    void StopSliding()
    {
        //if (!isJump)
        Debug.Log("�����̵� ����");
        if (isSliding)
        {
            isSliding = false;
            mainCollier.enabled = true; // ���� �ݶ��̴� Ȱ��ȭ
            slidingCollider.enabled = false; // �����̵� �ݶ��̴� ��Ȱ��ȭ
            animator.SetBool("isSliding", false);
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y); // X�� �ӵ� �ʱ�ȭ
            UpdateShield();
        }
    }

    //��Ʈ���� �̺�Ʈ
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.tag == "Enemy")
        {
            if (isShield)
            {
                Invoke("DeActivateShield", 0.5f); // �浹 ���� 0.5�ʵڿ� ��ȣ�� ����
                Debug.Log("���� �浹 �ǵ�������");
                return; // ���а� Ȱ��ȭ�Ǿ� ������ ���ظ� ���� ����
            }
            //Debug.Log("Player hit by enemy");
            OnDamaged(); //�ǰݽ� �Լ� ȣ��

        }

        else if (collision.CompareTag("MoveCheck"))
        {
            // MoveCheck�� ������ ���� Ȱ��ȭ
            isMoveCheck = true;
            Debug.Log("MoveCheck �浹, �÷��̾� ���� �� HP ���� Ȱ��ȭ!");
        }

    }

    public void ShieldSkill()
    {
        // Shield�� ������ ���� Ȱ��ȭ
        if (shieldCoroutine != null) StopCoroutine(shieldCoroutine);
        shieldCoroutine = StartCoroutine(ShieldTimer(7f));
        shieldObj.SetActive(true); // ���� ������Ʈ Ȱ��ȭ
        Debug.Log("�ǵ� ������ ȹ��: 7�ʰ� �ǵ� Ȱ��ȭ");
    }

    void OnDamaged()
    {
        gameObject.layer = 9; // "PlayerDamaged" ���̾�� ����

        spriteRenderer.color = new Color(1, 1, 1, 0.4f); // �ǰݽ� �����ϰ� ����
        animator.SetTrigger("isOnhit"); // �ǰ� �ִϸ��̼� Ʈ����
        playerstat.Hp -= damage; // HP ����
        Invoke("OffDamaged", 1); //1�� ���� �����Ŀ� ȣ��
    }

    void OffDamaged()
    {

        gameObject.layer = 6; // "Player" ���̾�� ����
        spriteRenderer.color = new Color(1, 1, 1, 1); // ���� ������� ����
    }


    //HP�� �ð��� �����鼭 �����ϴ� �Լ�
    void DecreaseHpOverTime()
    {
        if (playerstat.Hp > 0)
        {
            playerstat.Hp -= Time.deltaTime * hpDecreaseRate; // �ʴ� 0.5�� ����
            //Debug.Log("Player HP: " + playerstat.Hp);
        }
        else
        {
            isDead = true;
            animator.SetTrigger("isDie");
            _rigidbody.velocity = Vector2.zero; // �÷��̾� ����
            Debug.Log("Player is dead");
        }
    }

    // ���� y�� ���Ϸ� �������� ���� ó��
    void DeathTrigger()
    {
        isDead = true;
        mainCollier.enabled = false; // ���� �ݶ��̴� ��Ȱ��ȭ
        slidingCollider.enabled = false; // �����̵� �ݶ��̴� ��Ȱ��ȭ
        animator.SetTrigger("isDie");
        _rigidbody.velocity = Vector2.zero; // �÷��̾� ����
        _rigidbody.simulated = false;
    }

    void ActiveShield()
    {
        isShield = true;
        UpdateShield();
    }


    // ó�� �����Ҷ� �ʱ�ȭ
    void DeActivateShield()
    {
        isShield = false;
        if (mainshield != null)
        {
            mainshield.enabled = false; // ���� ���� �ݶ��̴� ��Ȱ��ȭ
        }
        if (slidingshield != null)
        {
            slidingshield.enabled = false; // �����̵� ���� �ݶ��̴� ��Ȱ��ȭ
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

    // HP ȸ�� �Լ�
    public void Heal(float amount)
    {
        playerstat.Hp += amount;
        if (playerstat.Hp > playerstat.maxHp)
        {
            playerstat.Hp = playerstat.maxHp; // �ִ� HP�� �ʰ����� �ʵ��� ����
        }
        Debug.Log("Player healed: " + amount + ", Current HP: " + playerstat.Hp);
    }


    IEnumerator ShieldTimer(float duration)
    {
        ActiveShield(); // ���� Ȱ��ȭ
        yield return new WaitForSeconds(duration); // ������ �ð� ���� ���
        DeActivateShield(); // ���� ��Ȱ��ȭ
        Debug.Log("�ǵ� ������ ȿ�� ����");
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
