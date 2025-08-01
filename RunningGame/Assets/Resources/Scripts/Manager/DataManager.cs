using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;

// 저장할 플레이어 데이터 구조체
[System.Serializable]
public class PlayerData
{
    public string userName;
    public int gold;
    public List<int> bastScores;
    public bool isSetName; //테스트용변수 서버에 값이 같이 저장되야함.

    public List<CharacterType> characters; // 현재는 캐릭터 타입 배열로 설정 (나중에 확장 가능성 있음)
    // 현재 장착중인 캐릭터
    public CharacterType currentCharacter;

    public PlayerData() // 초기 데이터 설정 (변경 해야 함)
    {
        userName = "NewPlayer";
        gold = 0;
        bastScores = new List<int>();
        bastScores.Add(0); // 0번째 배열은 버림 1번째 부터 사용
        isSetName = false;

        // 캐릭터 데이터 초기화
        characters = new List<CharacterType>();
        characters.Add(CharacterType.PlayerFishy); // 기본 캐릭터 추가

        // 현재 장착중인 캐릭터 초기화
        //currentCharacter = CharacterType.PlayerFishy; // 기본 캐릭터 설정
    }
}


public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private DatabaseReference dbReference;

    // 현재 플레이어 데이터 (로그인 후 불러온 데이터)
    public PlayerData currentPlayerdata;

    // 현재 플레이어가 선택한 던전
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
        currentPlayerdata = new PlayerData(); // 초기 플레이어 데이터 설정

        // Firebase Database의 루트 참조를 가져옵니다.
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        crrentDungeon = 0;
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
    public void SetCharacter(string characterName)
    {
        // 문자열을 CharacterType으로 변환 시도
        if (!Enum.TryParse<CharacterType>(characterName, out var characterType))
        {
            Debug.LogWarning("존재하지 않는 캐릭터 타입입니다: " + characterName);
            return;
        }

        if (currentPlayerdata.characters.Contains(characterType))
        {
            Debug.Log("이미 가지고 있는 캐릭터입니다: " + characterName);
        }
        else
        {
            // 캐릭터 추가
            currentPlayerdata.characters.Add(characterType);
            Debug.Log("새로운 캐릭터 추가됨: " + characterName);
        }
    }

    // 현재 캐릭터 설정 함수
    public void SetCurrentCharacter(CharacterType characterType)
    {
        currentPlayerdata.currentCharacter = characterType;
    }

    // 현재 던전 설정 (던전 들어갈 때 사용)
    public void GoingDungeon(int level)
    {
        crrentDungeon = level;
    }



    //---------------------------------------------------------------------------------------------------------------------


    // 버튼에 호출할 함수 (UI 함수로 가져가야 함) (0. PlayerFishy, 2. MaPlayerOrcyge, 3. PlayerPescy, 4. PlayerSharky)
    

    // GmamManager , UI(?)



}