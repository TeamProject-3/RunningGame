using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;

// ������ �÷��̾� ������ ����ü
[System.Serializable]
public class PlayerData
{
    public string userName;
    public int gold;
    public List<int> bastScores;
    public bool isSetName; //�׽�Ʈ�뺯�� ������ ���� ���� ����Ǿ���.

    public List<CharacterType> characters; // ����� ĳ���� Ÿ�� �迭�� ���� (���߿� Ȯ�� ���ɼ� ����)
    // ���� �������� ĳ����
    public CharacterType currentCharacter;

    public PlayerData() // �ʱ� ������ ���� (���� �ؾ� ��)
    {
        userName = "NewPlayer";
        gold = 0;
        bastScores = new List<int>();
        bastScores.Add(0); // 0��° �迭�� ���� 1��° ���� ���
        isSetName = false;

        // ĳ���� ������ �ʱ�ȭ
        characters = new List<CharacterType>();
        characters.Add(CharacterType.PlayerFishy); // �⺻ ĳ���� �߰�

        // ���� �������� ĳ���� �ʱ�ȭ
        //currentCharacter = CharacterType.PlayerFishy; // �⺻ ĳ���� ����
    }
}


public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private DatabaseReference dbReference;

    // ���� �÷��̾� ������ (�α��� �� �ҷ��� ������)
    public PlayerData currentPlayerdata;

    // ���� �÷��̾ ������ ����
    public int crrentDungeon;


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

        crrentDungeon = 0;
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
    public void SetCharacter(string characterName)
    {
        // ���ڿ��� CharacterType���� ��ȯ �õ�
        if (!Enum.TryParse<CharacterType>(characterName, out var characterType))
        {
            Debug.LogWarning("�������� �ʴ� ĳ���� Ÿ���Դϴ�: " + characterName);
            return;
        }

        if (currentPlayerdata.characters.Contains(characterType))
        {
            Debug.Log("�̹� ������ �ִ� ĳ�����Դϴ�: " + characterName);
        }
        else
        {
            // ĳ���� �߰�
            currentPlayerdata.characters.Add(characterType);
            Debug.Log("���ο� ĳ���� �߰���: " + characterName);
        }
    }

    // ���� ĳ���� ���� �Լ�
    public void SetCurrentCharacter(CharacterType characterType)
    {
        currentPlayerdata.currentCharacter = characterType;
    }

    // ���� ���� ���� (���� �� �� ���)
    public void GoingDungeon(int level)
    {
        crrentDungeon = level;
    }



    //---------------------------------------------------------------------------------------------------------------------


    // ��ư�� ȣ���� �Լ� (UI �Լ��� �������� ��) (0. PlayerFishy, 2. MaPlayerOrcyge, 3. PlayerPescy, 4. PlayerSharky)
    

    // GmamManager , UI(?)



}