using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnhit : MonoBehaviour
{

    Animator animator;
    Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

   

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Player hit by enemy");
        }
    }
}
