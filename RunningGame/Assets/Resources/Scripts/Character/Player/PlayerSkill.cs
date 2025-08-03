using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public bool isMagnet = false; // �ڼ� ��ų Ȱ��ȭ ����
    public float magnetRange = 5f; // �ڼ� ��ų ����
    public float magnetpower = 10f; // �ڼ� ��ų ��
    public float boostDuration = 5f; // �ν�Ʈ ���� �ð�
    public float boostSpeed = 30f; // �ν�Ʈ �ӵ�

    Coroutine magnetCoroutine; // �ڼ� ��ų�� Ȱ��ȭ�ϱ� ���� �ڷ�ƾ

    // Start is called before the first frame update

    void Update()
    {
        if (!isMagnet) return;

        GameObject[] jellies = GameObject.FindGameObjectsWithTag("Jelly"); // "Jelly" �±׸� ���� ��� ���� ������Ʈ�� ã���ϴ�.
        foreach (GameObject jelly in jellies)
        {
            float distance = Vector2.Distance(transform.position, jelly.transform.position);
            if (distance <= magnetRange)
            {
                Debug.Log("�ڼ� ��ų �۵� ��: " + jelly.name + "������ �Ÿ�: " + distance);
                Vector2 direction = (transform.position - jelly.transform.position).normalized; // �÷��̾�� ������ ���ϴ� ����
                Rigidbody2D jellyRb = jelly.GetComponent<Rigidbody2D>();
                if (jellyRb != null)
                {
                    jellyRb.AddForce(direction * magnetpower); // ������ �ڼ�ó�� ������ϴ�.
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
            // �ڼ� ��ų Ȱ��ȭ
            ActivateMagnet(5f); // 5�� ���� �ڼ� ��ų Ȱ��ȭ
            Destroy(collision.gameObject); // �ڼ� ������ ����
            Debug.Log("�ڼ� ��ų Ȱ��ȭ!");
        }

        if(collision.CompareTag("Jelly"))
        {
           Destroy(collision.gameObject); // ���� ����
        }

        if (collision.CompareTag("ItemBoost"))
        {
            // ���� ������ ȹ��
            Debug.Log("�ν�Ʈ 5�ʰ� Ȱ��ȭ");
            Destroy(collision.gameObject); // ���� ������ ����
        }
    }

    public void ActivateBoost()
    {

        PlayerStat playerStat = GetComponent<PlayerStat>(); 
        Player player = GetComponent<Player>();
        InGameManager inGame = InGameManager.Instance;
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

