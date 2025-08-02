using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

// ��ŷ ����
public struct UserRank
{
    public string username;
    public int score;
    public int dungeonIndex; // �߰�: � ������ ��������
}

public class RankingManager : MonoBehaviour
{
    [Tooltip("��ŷ �׸��� �߰��� UI Panel�� Transform")]
    public Transform rankingContent; // ��ŷ �׸��� �߰��� UI Panel�� Transform
    [Tooltip("���� ����, �̸�, ������ ǥ���� UI ������ ������")]
    public GameObject rankingItemPrefab; // ���� ����, �̸�, ������ ǥ���� UI ������

    private DatabaseReference dbReference;

    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        ShowRanking(1); // �⺻ 0�� ���� ��ŷ ǥ��
    }

    // ���� N���� ��ŷ�� �ҷ����� �Լ� (���� �ε�����)
    public async Task<List<UserRank>> GetTopRankings(int limit, int dungeonIndex)
    {
        List<UserRank> rankingList = new List<UserRank>();

        try
        {
            // ��� ���� ������ ��������
            var snapshot = await dbReference.Child("users").GetValueAsync();

            if (!snapshot.Exists)
            {
                Debug.Log("����Ʈ ����");
                return rankingList;
            }

            foreach (var childSnapshot in snapshot.Children)
            {
                var userDict = (IDictionary<string, object>)childSnapshot.Value;

                string username = userDict.ContainsKey("userName") ? userDict["userName"].ToString() : "Unknown";
                int score = 0;

                // bastScores ����Ʈ���� dungeonIndex�� ���� ��������
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

            // ���� �������� ���� �� ���� limit�� ����
            rankingList = rankingList.OrderByDescending(u => u.score).Take(limit).ToList();

            Debug.Log("Successfully fetched ranking data.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to get ranking data: {e.Message}");
        }

        return rankingList;
    }

    // UI�� ��ŷ�� ǥ���ϴ� �Լ�
    public void DisplayRankings(List<UserRank> rankingList)
    {
        // ���� ��ŷ UI ����
        foreach (Transform child in rankingContent)
        {
            Destroy(child.gameObject);
        }

        // ���ο� ��ŷ UI ����
        for (int i = 0; i < rankingList.Count; i++)
        {
            GameObject itemGO = Instantiate(rankingItemPrefab, rankingContent);
            // �������� Text ������Ʈ�� �����Ͽ� ���� ���� (������ ������ ���� �ڵ� ���� �ʿ�)
            Text rankText = itemGO.transform.Find("RankText").GetComponent<Text>();
            Text nameText = itemGO.transform.Find("NameText").GetComponent<Text>();
            Text scoreText = itemGO.transform.Find("ScoreText").GetComponent<Text>();

            rankText.text = (i + 1).ToString();
            nameText.text = rankingList[i].username;
            scoreText.text = string.Format("{0:N0}", rankingList[i].score);
        }
    }

    // ������ ��ŷ�� �����ִ� �Լ��� ����
    public async void ShowRanking(int dungeonIndex)
    {
        var top = await GetTopRankings(3, dungeonIndex); // ���� 10�� ��ŷ �ҷ�����
        DisplayRankings(top);
    }
}