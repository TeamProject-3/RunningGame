using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

// ������ �÷��̾� ������ ����ü
[System.Serializable]
public class PlayerData
{
    public string userName;
    public int gold;
    public int bastScore;
    // ĳ���ʹ� Dictionary (ĳ���� �̸�(ĳ����ID), ĳ���� ������(ĳ���� ������?))
    // ���� �������� ĳ����
    public PlayerData() // �ʱ� ������ ���� (���� �ؾ� ��)
    {
        userName = "NewPlayer";
        gold = 0;
        bastScore = 0;
        // ĳ���� ������ �ʱ�ȭ (���÷� �� ��ųʸ� ���)
        // ���� �������� ĳ���� �ʱ�ȭ
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
        currentPlayerdata = null; // �ʱ�ȭ
    }

    void Start()
    {
        // Firebase Database�� ��Ʈ ������ �����ɴϴ�.
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
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

    public async void OnSaveDataButtonClicked()
    {
        // 1. AuthManager�κ��� ���� ������ UID�� �����ɴϴ�.
        string uid = FIrebaseAuthManager.Instance.GetUserUID();

        // 2. UID�� �ִ��� Ȯ�� (�α��� �������� üũ)
        if (string.IsNullOrEmpty(uid))
        {
            Debug.LogWarning("�α��� ���°� �ƴմϴ�. �����͸� ������ �� �����ϴ�.");
                return;
        }
        // 3. uid�� ����
        await SaveData(uid);
    }
}