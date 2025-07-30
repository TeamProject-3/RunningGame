using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSliding : MonoBehaviour
{
    Animator animator;
    Rigidbody2D _rigidbody;
    public float slideSpeed = 5f;
    public bool isSliding = false;
    public bool isDead = false;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {

        }
        else
        {
            if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(2))
            {
                StartSliding();
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetMouseButtonUp(2))
            {
                StopSliding();
            }
        }
    }


    void StartSliding()
    {
        if (!isSliding)
        {
            isSliding = true;
            animator.SetBool("isSliding", true);
            _rigidbody.velocity = new Vector2(slideSpeed, _rigidbody.velocity.y); // X축 속도 설정
        }
    }

    void StopSliding()
    {
        if (isSliding)
        {
            isSliding = false;
            animator.SetBool("isSliding", false);
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y); // X축 속도 초기화
        }
    }
}
