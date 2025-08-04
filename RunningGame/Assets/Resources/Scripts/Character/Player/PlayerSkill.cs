using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public bool isMagnet = false; // 자석 스킬 활성화 여부
    public float magnetRange = 5f; // 자석 스킬 범위
    public float magnetpower = 10f; // 자석 스킬 힘
    public float boostDuration = 5f; // 부스트 지속 시간
    public float boostSpeed = 30f; // 부스트 속도

    Coroutine magnetCoroutine; // 자석 스킬을 활성화하기 위한 코루틴

    //부스터 중일때 부스터 아이템 먹었을때 저장하는 불값 변수
    public bool isBoostItem = false; // 부스터 아이템을 먹었는지 여부

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


    public void ActivateBoost()
    {
        PlayerStat playerStat = GetComponent<PlayerStat>();
        Player player = GetComponent<Player>();
        InGameManager inGame = InGameManager.Instance;

        if (player.isSBoost)
        {
            isBoostItem = true;
            return; // 이미 부스트 중이면 중복 실행 방지
        }


        inGame.currentSpeed = playerStat.moveSpeed;
        inGame.boostColliderPrefab.SetActive(true);
        inGame.SetSpeed(boostSpeed);
        player.isSBoost = true;
        player.gameObject.layer = 9;
        Invoke("BoostEnd", boostDuration);
    }

    private void BoostEnd()
    {
        PlayerStat playerStat = GetComponent<PlayerStat>();
        Player player = GetComponent<Player>();
        InGameManager inGame = InGameManager.Instance;
        player.isSBoost = false;
        inGame.SetSpeed(inGame.currentSpeed);
        if(isBoostItem)
        {
            isBoostItem = false; // 부스터 아이템을 먹었음을 초기화
            ActivateBoost();
        }
        else
            Invoke("BoostColliderOff", 1);
    }

    private void BoostColliderOff()
    {
        InGameManager inGame = InGameManager.Instance;
        Player player = GetComponent<Player>();
        inGame.boostColliderPrefab.SetActive(false);
        player.gameObject.layer = 6;
    }

}

