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
        
    }
}
