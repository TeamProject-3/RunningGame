using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    Rigidbody2D _rigidbody;

    public float jumpForce = 7f;
    public int jumpCount = 0;
    public int maxJumpCount = 2; // Maximum number of jumps allowed
    public bool isDead = false;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found in children of Player.");
        }

        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody2D component not found on Player.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (jumpCount < maxJumpCount)
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0); // Y축 속도 초기화
                    _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    jumpCount++;
                    animator.SetTrigger("Jump");
                    animator.SetBool("Jump", true);
                   
                }
            }
        }
    }


    private void FixedUpdate()
    {
        if (_rigidbody.velocity.y < 0)
        {
            animator.SetBool("Jump", true);
            Debug.DrawRay(_rigidbody.position + new Vector2(-4.8f, -4.2f), Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayhit = Physics2D.Raycast(_rigidbody.position + new Vector2(-4.8f, -4.2f), Vector2.down, 2, LayerMask.GetMask("Ground"));
            if (rayhit.collider != null)
            {
                if (rayhit.distance < 1)
                {
                    jumpCount = 0;
                    animator.SetBool("Jump", false);
                    Debug.Log("Raycast hit: " + rayhit.collider.name);
                }
            }
        }
    }


}
