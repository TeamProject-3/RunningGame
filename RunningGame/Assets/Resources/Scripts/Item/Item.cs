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
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                ApplyEffect(player);
            }
            gameObject.SetActive(false); // �������� ��Ȱ��ȭ
        }
    }

    //�����ۺ� ȿ�� ����
    private void ApplyEffect(Player player)
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
                break;
            case ItemType.Shield:
                //player.ActivateShield(value);
                break;
                // �߰� ������ ȿ�� ����
        }
    }
}