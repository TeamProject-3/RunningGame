using Unity.VisualScripting;
using UnityEngine;

public class Spring : MonoBehaviour
{
    Animator animator;
    [SerializeField] float jumpForce = 0.5f;
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
            PlayerStat playerStat = collision.gameObject.GetComponent< PlayerStat>();
            if (_rigidbody == null) return;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
            _rigidbody.AddForce(Vector2.up * playerStat.jumpForce +  new Vector2(jumpForce * playerStat.jumpForce,0), ForceMode2D.Impulse);
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
