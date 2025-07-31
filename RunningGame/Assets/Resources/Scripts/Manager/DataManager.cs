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

// 저장할 플레이어 데이터 구조체
[System.Serializable]
public class PlayerData
{
    public string userName;
    public int gold;
    public int bastScore;
    public bool isSetName; //테스트용변수 서버에 값이 같이 저장되야함.
    // 캐릭터는 Dictionary (캐릭터 이름(캐릭터ID), 캐릭터 데이터(캐릭터 프리팹?))

    // 현재 장착중인 캐릭터
    public CharacterType currentCharacter;

    public PlayerData() // 초기 데이터 설정 (변경 해야 함)
    {
        userName = "NewPlayer";
        gold = 0;
        bastScore = 0;
        isSetName = false;
        // 캐릭터 데이터 초기화 (예시로 빈 딕셔너리 사용)

        // 현재 장착중인 캐릭터 초기화
        currentCharacter = CharacterType.PlayerFishy; // 기본 캐릭터 설정
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
        currentPlayerdata = new PlayerData(); // 초기 플레이어 데이터 설정

        // Firebase Database의 루트 참조를 가져옵니다.
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void Start()
    {
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

    public async void OnSaveData()
    {
        // AuthManager로부터 현재 유저의 UID
        string uid = FIrebaseAuthManager.Instance.GetUserUID();

        // UID가 있는지 확인 (로그인 상태인지 체크)
        if (string.IsNullOrEmpty(uid))
        {
            Debug.LogWarning("로그인 상태가 아닙니다. 데이터를 저장할 수 없습니다.");
                return;
        }

        // uid로 저장
        await SaveData(uid);
    }

    // 이름 설정 함수
    public void SetName(string name)
    {
        if(currentPlayerdata.isSetName == false)
        {
            currentPlayerdata.userName = name;
            currentPlayerdata.isSetName = true;
        }
    }

    // 내가 가지고 있는 캐릭터 추가
    public void SetCharacter()
    {

    }

    // 현재 캐릭터 설정 함수
    public void SetCurrentCharacter(CharacterType characterType)
    {
        currentPlayerdata.currentCharacter = characterType;
    }

    //---------------------------------------------------------------------------------------------------------------------


    // 버튼에 호출할 함수 (UI 함수로 가져가야 함) (0. PlayerFishy, 2. MaPlayerOrcyge, 3. PlayerPescy, 4. PlayerSharky)
    public void ChangeCharacter(int characterIndex)
    {
        if(currentPlayerdata.currentCharacter == (CharacterType)characterIndex)
        {
            Debug.Log("이미 선택된 캐릭터입니다: " + (CharacterType)characterIndex);
            // 이미 선택된 캐릭터라고 띄우는 UI????
            return;
        }
        else
        {
            // DataManager.Instance.SetCurrentCharacter(characterIndex);
            SetCurrentCharacter((CharacterType)characterIndex);


            Debug.Log("캐릭터 변경됨: " + (CharacterType)characterIndex);

            // 캐릭터 변경 UI 업데이트
            // GameManager.Instance.ChangeCharacterImage();
            ChangeCharacterImage();
        }
    }

    // GmamManager , UI(?)
    [SerializeField]
    private GameObject changeCharacters;

    public void ChangeCharacterImage()
    {
        // 자식 오브젝트 가져옴 
        Transform[] gameObjects = changeCharacters.GetComponentsInChildren<Transform>(true);


        foreach (Transform t in gameObjects)
        {
            // 부모 오브젝트는 건너뜀
            if (t == changeCharacters.transform)
                continue;

            // 캐릭터 이름과 현재 플레이어 데이터의 캐릭터 이름 비교
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