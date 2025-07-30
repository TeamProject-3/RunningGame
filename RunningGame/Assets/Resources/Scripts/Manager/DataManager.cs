using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

// 저장할 플레이어 데이터 구조체
[System.Serializable]
public class PlayerData
{
    public string userName;
    public int gold;
    public int bastScore;
    // 캐릭터는 Dictionary (캐릭터 이름(캐릭터ID), 캐릭터 데이터(캐릭터 프리팹?))
    // 현재 장착중인 캐릭터
    public PlayerData() // 초기 데이터 설정 (변경 해야 함)
    {
        userName = "NewPlayer";
        gold = 0;
        bastScore = 0;
        // 캐릭터 데이터 초기화 (예시로 빈 딕셔너리 사용)
        // 현재 장착중인 캐릭터 초기화
    }
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private DatabaseReference dbReference;

    // 현재 플레이어 데이터 (로그인 후 불러온 데이터)
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
        currentPlayerdata = null; // 초기화
    }

    void Start()
    {
        // Firebase Database의 루트 참조를 가져옵니다.
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // 데이터 저장
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
            Debug.LogError($"세이브 에러: {e.Message}");
        }
    }

    // 데이터 로드
    public async Task<PlayerData> LoadData(string uid)
    {
        if (string.IsNullOrEmpty(uid)) return null;

        try
        {
            var dataSnapshot = await dbReference.Child("users").Child(uid).GetValueAsync();

            // 만약 데이터가 존재하지 않는다면 기본값을 반환
            if (!dataSnapshot.Exists)
            {
                Debug.Log("No data found for this user. Creating new data.");
                return new PlayerData();
            }

            string jsonData = dataSnapshot.GetRawJsonValue();
            PlayerData data = JsonUtility.FromJson<PlayerData>(jsonData);
            Debug.Log("데이터 로드 완료");
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"데이터 로드 에러: {e.Message}");
            return null;
        }
    }

    public async void OnSaveDataButtonClicked()
    {
        // 1. AuthManager로부터 현재 유저의 UID를 가져옵니다.
        string uid = FIrebaseAuthManager.Instance.GetUserUID();

        // 2. UID가 있는지 확인 (로그인 상태인지 체크)
        if (string.IsNullOrEmpty(uid))
        {
            Debug.LogWarning("로그인 상태가 아닙니다. 데이터를 저장할 수 없습니다.");
                return;
        }
        // 3. uid로 저장
        await SaveData(uid);
    }
}