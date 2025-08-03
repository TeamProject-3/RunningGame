using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Coin,
    Magnet,
    Heart,
    Boost,
    Food,
    Shield

    // �ʿ信 ���� �߰� ����
}

public class Item : MonoBehaviour
{
    public ItemType type;   // ������ ����
    public float value;     // ���� �Ǵ� ȿ���� ũ�� (��: ȸ����, ���ӽð�)

    //�÷��̾�� ���� �� ȿ�� �ߵ�
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyEffect();
            gameObject.SetActive(false); // �������� ��Ȱ��ȭ
        }
    }

    //�����ۺ� ȿ�� ����
    private void ApplyEffect()
    {
        switch (type)
        {
            case ItemType.Coin:
                //player.AddMoney((int)value);
                break;
            case ItemType.Magnet:
                //player.ActivateMagnet(value);
                break;
            case ItemType.Heart:
                //player.Heal((int)value);
                break;
            case ItemType.Boost:
                //player.ActivateBoost(value);
                break;
            case ItemType.Food:
                InGameManager.Instance.IncreaseScore(value);
                if(Random.Range(0, 100) < 1f)
                {
                    InGameManager.Instance.coinCount += 100;
                    UIManager_InGame.Instance.coinCount += InGameManager.Instance.coinCount;
                    UIManager_InGame.Instance.UpdateCoinText();
                }
                break;
            case ItemType.Shield:
                //player.ActivateShield(value);
                break;
                // �߰� ������ ȿ�� ����
        }
    }
}