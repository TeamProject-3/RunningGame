using UnityEngine;

public class Spring : MonoBehaviour
{
    Animator animator;

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
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
            _rigidbody.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        animator.SetBool("IsMove", false);
    }


}
