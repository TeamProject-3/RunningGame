using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;
public enum CharacterType
{
    PlayerFishy,
    PlayerOrcy,
    PlayerPescy,
    PlayerSharky
}

// ������ �÷��̾� ������ ����ü
[System.Serializable]
public class PlayerData
{
    public string userName;
    public int gold;
    public int bastScore;
    public bool isSetName; //�׽�Ʈ�뺯�� ������ ���� ���� ����Ǿ���.
    // ĳ���ʹ� Dictionary (ĳ���� �̸�(ĳ����ID), ĳ���� ������(ĳ���� ������?))

    // ���� �������� ĳ����
    public CharacterType currentCharacter;

    public PlayerData() // �ʱ� ������ ���� (���� �ؾ� ��)
    {
        userName = "NewPlayer";
        gold = 0;
        bastScore = 0;
        isSetName = false;
        // ĳ���� ������ �ʱ�ȭ (���÷� �� ��ųʸ� ���)

        // ���� �������� ĳ���� �ʱ�ȭ
        currentCharacter = CharacterType.PlayerFishy; // �⺻ ĳ���� ����
    }
}


public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private DatabaseReference dbReference;

    // ���� �÷��̾� ������ (�α��� �� �ҷ��� ������)
    public PlayerData currentPlayerdata;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        currentPlayerdata = new PlayerData(); // �ʱ� �÷��̾� ������ ����

        // Firebase Database�� ��Ʈ ������ �����ɴϴ�.
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void Start()
    {
    }

    // ������ ����
    public async Task SaveData(string uid)
    {
        if (string.IsNullOrEmpty(uid)) return;

        string jsonData = JsonUtility.ToJson(currentPlayerdata);
        try
        {
            await dbReference.Child("users").Child(uid).SetRawJsonValueAsync(jsonData);
            Debug.Log("Data saved successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"���̺� ����: {e.Message}");
        }
    }

    // ������ �ε�
    public async Task<PlayerData> LoadData(string uid)
    {
        if (string.IsNullOrEmpty(uid)) return null;

        try
        {
            var dataSnapshot = await dbReference.Child("users").Child(uid).GetValueAsync();

            // ���� �����Ͱ� �������� �ʴ´ٸ� �⺻���� ��ȯ
            if (!dataSnapshot.Exists)
            {
                Debug.Log("No data found for this user. Creating new data.");
                return new PlayerData();
            }

            string jsonData = dataSnapshot.GetRawJsonValue();
            PlayerData data = JsonUtility.FromJson<PlayerData>(jsonData);
            Debug.Log("������ �ε� �Ϸ�");
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"������ �ε� ����: {e.Message}");
            return null;
        }
    }

    public async void OnSaveData()
    {
        // AuthManager�κ��� ���� ������ UID
        string uid = FIrebaseAuthManager.Instance.GetUserUID();

        // UID�� �ִ��� Ȯ�� (�α��� �������� üũ)
        if (string.IsNullOrEmpty(uid))
        {
            Debug.LogWarning("�α��� ���°� �ƴմϴ�. �����͸� ������ �� �����ϴ�.");
                return;
        }

        // uid�� ����
        await SaveData(uid);
    }

    // �̸� ���� �Լ�
    public void SetName(string name)
    {
        if(currentPlayerdata.isSetName == false)
        {
            currentPlayerdata.userName = name;
            currentPlayerdata.isSetName = true;
        }
    }

    // ���� ������ �ִ� ĳ���� �߰�
    public void SetCharacter()
    {

    }

    // ���� ĳ���� ���� �Լ�
    public void SetCurrentCharacter(CharacterType characterType)
    {
        currentPlayerdata.currentCharacter = characterType;
    }

    //---------------------------------------------------------------------------------------------------------------------


    // ��ư�� ȣ���� �Լ� (UI �Լ��� �������� ��) (0. PlayerFishy, 2. MaPlayerOrcyge, 3. PlayerPescy, 4. PlayerSharky)
    public void ChangeCharacter(int characterIndex)
    {
        if(currentPlayerdata.currentCharacter == (CharacterType)characterIndex)
        {
            Debug.Log("�̹� ���õ� ĳ�����Դϴ�: " + (CharacterType)characterIndex);
            // �̹� ���õ� ĳ���Ͷ�� ���� UI????
            return;
        }
        else
        {
            // DataManager.Instance.SetCurrentCharacter(characterIndex);
            SetCurrentCharacter((CharacterType)characterIndex);


            Debug.Log("ĳ���� �����: " + (CharacterType)characterIndex);

            // ĳ���� ���� UI ������Ʈ
            // GameManager.Instance.ChangeCharacterImage();
            ChangeCharacterImage();
        }
    }

    // GmamManager , UI(?)
    [SerializeField]
    private GameObject changeCharacters;

    public void ChangeCharacterImage()
    {
        // �ڽ� ������Ʈ ������ 
        Transform[] gameObjects = changeCharacters.GetComponentsInChildren<Transform>(true);


        foreach (Transform t in gameObjects)
        {
            // �θ� ������Ʈ�� �ǳʶ�
            if (t == changeCharacters.transform)
                continue;

            // ĳ���� �̸��� ���� �÷��̾� �������� ĳ���� �̸� ��
            if (t.name == currentPlayerdata.currentCharacter.ToString())
            {
                t.gameObject.SetActive(true);
            }
            else
            {
                t.gameObject.SetActive(false);
            }
        }
    }


}