using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public bool isMagnet = false; // 자석 스킬 활성화 여부
    public float magnetRange = 5f; // 자석 스킬 범위
    public float magnetpower = 10f; // 자석 스킬 힘

    Coroutine magnetCoroutine; // 자석 스킬을 활성화하기 위한 코루틴

    // Start is called before the first frame update

    void Update()
    {
        if (!isMagnet) return;

        GameObject[] jellies = GameObject.FindGameObjectsWithTag("Jelly"); // "Jelly" 태그를 가진 모든 젤리 오브젝트를 찾습니다.
        foreach (GameObject jelly in jellies)
        {
            float distance = Vector2.Distance(transform.position, jelly.transform.position);
            if (distance <= magnetRange)
            {
                Debug.Log("자석 스킬 작동 중: " + jelly.name + "까지의 거리: " + distance);
                Vector2 direction = (transform.position - jelly.transform.position).normalized; // 플레이어에서 젤리로 향하는 방향
                Rigidbody2D jellyRb = jelly.GetComponent<Rigidbody2D>();
                if (jellyRb != null)
                {
                    jellyRb.AddForce(direction * magnetpower); // 젤리를 자석처럼 끌어당깁니다.
                }
            }
        }
    }
    public void ActivateMagnet(float duration)
    {
        if (magnetCoroutine != null)
        {
            StopCoroutine(magnetCoroutine);
        }
        magnetCoroutine = StartCoroutine(magnetTimer(duration));
    }

    IEnumerator magnetTimer(float duration)
    {
        isMagnet = true;
        yield return new WaitForSeconds(duration);
        isMagnet = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ItemMagnet"))
        {
            // 자석 스킬 활성화
            ActivateMagnet(5f); // 5초 동안 자석 스킬 활성화
            Destroy(collision.gameObject); // 자석 아이템 제거
            Debug.Log("자석 스킬 활성화!");
        }

        if(collision.CompareTag("Jelly"))
        {
           Destroy(collision.gameObject); // 젤리 제거
        }

        if (collision.CompareTag("ItemBoost"))
        {
            // 젤리 아이템 획득
            Debug.Log("부스트 5초간 활성화");
            Destroy(collision.gameObject); // 젤리 아이템 제거
        }
    }
}

