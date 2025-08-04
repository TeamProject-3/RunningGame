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
            ApplyEffect();
            gameObject.SetActive(false); // 아이템을 비활성화
        }
    }

    //아이템별 효과 적용
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
                {
                    Player player = FindObjectOfType<Player>();
                    player.Heal(value);
                }
                break;
            case ItemType.Boost:
                {
                    PlayerSkill player = FindObjectOfType<PlayerSkill>();
                    player.ActivateBoost();
                }
                break;
            case ItemType.Food:
                InGameManager.Instance.IncreaseScore(value);
                if (Random.Range(0, 100) < 1f)
                {
                    InGameManager.Instance.coinCount += 100;
                    UIManager_InGame.Instance.coinCount += InGameManager.Instance.coinCount;
                    UIManager_InGame.Instance.UpdateCoinText();
                }
                break;
            case ItemType.Shield:
                {
                    Player player = FindObjectOfType<Player>();
                    player.ShieldSkill();
                }
                break;
                // 추가 아이템 효과 구현
        }
    }
}