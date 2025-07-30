using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Playermove : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도
    private Rigidbody2D _rigidbody;
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.velocity = new Vector2(moveSpeed, _rigidbody.velocity.y);
    }
}
