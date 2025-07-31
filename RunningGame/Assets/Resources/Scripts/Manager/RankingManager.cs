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
        ShowRanking();
    }

    // 상위 N명의 랭킹을 불러오는 함수
    public async Task<List<UserRank>> GetTopRankings(int limit)
    {
        List<UserRank> rankingList = new List<UserRank>();

        try
        {
            // "users" 경로에서 "score"를 기준으로 내림차순 정렬 후 상위 limit개 만큼 가져오기
            var snapshot = await dbReference.Child("users").OrderByChild("bastScore").LimitToLast(limit).GetValueAsync();

            // 스냅샷이 존재하지 않으면 빈 리스트 반환
            if (!snapshot.Exists)
            {
                Debug.Log("리스트 없음");
                return rankingList;
            }

            // 가져온 데이터를 UserRank 객체로 변환하여 리스트에 추가
            foreach (var childSnapshot in snapshot.Children)
            {
                var userDict = (IDictionary<string, object>)childSnapshot.Value;
                UserRank userRank = new UserRank
                {
                    username = userDict["userName"].ToString(),
                    score = int.Parse(userDict["bastScore"].ToString())
                };
                rankingList.Add(userRank);
            }

            // Firebase는 오름차순으로 가져옴, 내림차순으로 정렬
            rankingList.Reverse();

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
            scoreText.text = rankingList[i].score.ToString();
        }
    }

    // 랭킹에  대한 버튼 클릭 이벤트 핸들러
    public async void ShowRanking()
    {
        var top10 = await GetTopRankings(10); // 상위 10명 랭킹 불러오기
        DisplayRankings(top10);
    }
}