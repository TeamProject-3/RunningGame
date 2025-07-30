using System.Collections;
using UnityEngine;

public class JellyFish : MonoBehaviour
{
    Animator[] animators;
    GameObject player;

    Vector3 positions;

    private float speed = 0.02f;
    bool updateCheck = false;

    private void Awake()
    {
        player = FindObjectOfType<Playermove>().gameObject;
        animators = GetComponentsInChildren<Animator>();
    }

    
    void Update()
    {
        if (updateCheck)
            if (player.transform.position.x > positions.x + 2)
            {
                foreach (Animator animator in animators)
                    animator.SetBool("IsMove", true);

                transform.position += new Vector3(0, speed, 0);
            }
        else
            foreach (Animator animator in animators)
                animator.SetBool("IsMove", false);
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
