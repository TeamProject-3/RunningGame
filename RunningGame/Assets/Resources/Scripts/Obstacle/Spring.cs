using UnityEngine;

public class Spring : MonoBehaviour
{
    Animator animator;
    [SerializeField] float jumpBaseForce = 1f;

    float jumpChangeForce;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("IsMove", true);
            Player player = FindAnyObjectByType<Player>();
            Rigidbody2D _rigidbody = player.gameObject.GetComponent<Rigidbody2D>();
            PlayerStat playerStat = player.gameObject.GetComponent<PlayerStat>();
            if (_rigidbody == null) return;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);

            jumpChangeForce = (jumpBaseForce * playerStat.jumpForce) / playerStat.baseJumpForce;

            _rigidbody.AddForce(Vector2.up * playerStat.jumpForce +  
                new Vector2(0, jumpChangeForce), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("IsMove", false);
        }
    }


}
