using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomeber : MonoBehaviour
{
    Animator[] animators;
    GameObject player;
    Rigidbody2D[] rigidbodyOne;
    Vector3 positions;

    private float speed = 0.02f;
    bool updateCheck = false;

    private void Awake()
    {
        player = FindObjectOfType<Player>().gameObject;
        animators = GetComponentsInChildren<Animator>();
        rigidbodyOne = GetComponentsInChildren<Rigidbody2D>();
    }


    void Update()
    {
        if (updateCheck)
            if (player.transform.position.x > positions.x - 5)
            {
                foreach (Animator animator in animators)
                    animator.SetBool("IsBoom", true);

                foreach (Rigidbody2D rigid in rigidbodyOne)
                    rigid.bodyType = RigidbodyType2D.Dynamic;
            }
            else
            { 
                foreach (Animator animator in animators)
                    animator.SetBool("IsBoom", false);
                foreach (Rigidbody2D rigid in rigidbodyOne)
                    rigid.bodyType = RigidbodyType2D.Kinematic;
            }

        if (positions.y < -2.72f) positions.y = -2.72f;
    }


    private void OnEnable()
    {
        updateCheck = true;
        StartCoroutine(Delays());
        transform.localPosition = Vector3.zero;
    }

    private void OnDisable()
    {
        updateCheck = false;
    }

    private IEnumerator Delays()
    {
        yield return null;
        if (transform.parent != null)
            positions = transform.parent.position;

    }
}
