using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   // Animator animator;
    Rigidbody2D _rigidbody;

    public float forwardSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
