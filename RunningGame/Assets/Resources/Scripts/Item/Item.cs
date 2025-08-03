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

    // 필요에 따라 추가 가능
}

public class Item : MonoBehaviour
{
    public ItemType type;   // 아이템 종류
    public float value;     // 점수 또는 효과의 크기 (예: 회복량, 지속시간)

    //플레이어와 접촉 시 효과 발동
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                ApplyEffect(player);
            }
            gameObject.SetActive(false); // 아이템을 비활성화
        }
    }

    //아이템별 효과 적용
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
                // 추가 아이템 효과 구현
        }
    }
}