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
                // �߰� ������ ȿ�� ����
        }
    }
}