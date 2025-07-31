using UnityEngine;

public class Spring : MonoBehaviour
{
    Animator animator;
    [SerializeField] float jumpHigh = 8f;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("IsMove", true);
            Rigidbody2D _rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (_rigidbody == null) return;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
            _rigidbody.AddForce(Vector2.up * jumpHigh, ForceMode2D.Impulse);
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
