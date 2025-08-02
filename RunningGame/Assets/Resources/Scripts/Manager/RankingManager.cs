using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

// 랭킹 정보
public struct UserRank
{
    public string username;
    public int score;
    public int dungeonIndex; // 추가: 어떤 던전의 점수인지
}

public class RankingManager : MonoBehaviour
{
    [Tooltip("랭킹 항목이 추가될 UI Panel의 Transform")]
    public Transform rankingContent; // 랭킹 항목이 추가될 UI Panel의 Transform
    [Tooltip("유저 순위, 이름, 점수를 표시할 UI 프리팹 프리팹")]
    public GameObject rankingItemPrefab; // 유저 순위, 이름, 점수를 표시할 UI 프리팹

    private DatabaseReference dbReference;

    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        ShowRanking(1); // 기본 0번 던전 랭킹 표시
    }

    // 상위 N명의 랭킹을 불러오는 함수 (던전 인덱스별)
    public async Task<List<UserRank>> GetTopRankings(int limit, int dungeonIndex)
    {
        List<UserRank> rankingList = new List<UserRank>();

        try
        {
            // 모든 유저 데이터 가져오기
            var snapshot = await dbReference.Child("users").GetValueAsync();

            if (!snapshot.Exists)
            {
                Debug.Log("리스트 없음");
                return rankingList;
            }

            foreach (var childSnapshot in snapshot.Children)
            {
                var userDict = (IDictionary<string, object>)childSnapshot.Value;

                string username = userDict.ContainsKey("userName") ? userDict["userName"].ToString() : "Unknown";
                int score = 0;

                // bastScores 리스트에서 dungeonIndex의 점수 가져오기
                if (userDict.ContainsKey("bastScores"))
                {
                    var scoresObj = userDict["bastScores"] as List<object>;
                    if (scoresObj != null && dungeonIndex < scoresObj.Count)
                    {
                        int.TryParse(scoresObj[dungeonIndex].ToString(), out score);
                    }
                }

                UserRank userRank = new UserRank
                {
                    username = username,
                    score = score,
                    dungeonIndex = dungeonIndex
                };
                rankingList.Add(userRank);
            }

            // 점수 내림차순 정렬 후 상위 limit만 추출
            rankingList = rankingList.OrderByDescending(u => u.score).Take(limit).ToList();

            Debug.Log("Successfully fetched ranking data.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to get ranking data: {e.Message}");
        }

        return rankingList;
    }

    // UI에 랭킹을 표시하는 함수
    public void DisplayRankings(List<UserRank> rankingList)
    {
        // 기존 랭킹 UI 삭제
        foreach (Transform child in rankingContent)
        {
            Destroy(child.gameObject);
        }

        // 새로운 랭킹 UI 생성
        for (int i = 0; i < rankingList.Count; i++)
        {
            GameObject itemGO = Instantiate(rankingItemPrefab, rankingContent);
            // 프리팹의 Text 컴포넌트에 접근하여 정보 설정 (프리팹 구조에 따라 코드 수정 필요)
            Text rankText = itemGO.transform.Find("RankText").GetComponent<Text>();
            Text nameText = itemGO.transform.Find("NameText").GetComponent<Text>();
            Text scoreText = itemGO.transform.Find("ScoreText").GetComponent<Text>();

            rankText.text = (i + 1).ToString();
            nameText.text = rankingList[i].username;
            scoreText.text = string.Format("{0:N0}", rankingList[i].score);
        }
    }

    // 던전별 랭킹을 보여주는 함수로 변경
    public async void ShowRanking(int dungeonIndex)
    {
        var top = await GetTopRankings(3, dungeonIndex); // 상위 10명 랭킹 불러오기
        DisplayRankings(top);
    }
}