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
        ShowRanking();
    }

    // ���� N���� ��ŷ�� �ҷ����� �Լ�
    public async Task<List<UserRank>> GetTopRankings(int limit)
    {
        List<UserRank> rankingList = new List<UserRank>();

        try
        {
            // "users" ��ο��� "score"�� �������� �������� ���� �� ���� limit�� ��ŭ ��������
            var snapshot = await dbReference.Child("users").OrderByChild("bastScore").LimitToLast(limit).GetValueAsync();

            // �������� �������� ������ �� ����Ʈ ��ȯ
            if (!snapshot.Exists)
            {
                Debug.Log("����Ʈ ����");
                return rankingList;
            }

            // ������ �����͸� UserRank ��ü�� ��ȯ�Ͽ� ����Ʈ�� �߰�
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

            // Firebase�� ������������ ������, ������������ ����
            rankingList.Reverse();

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
            scoreText.text = rankingList[i].score.ToString();
        }
    }

    // ��ŷ��  ���� ��ư Ŭ�� �̺�Ʈ �ڵ鷯
    public async void ShowRanking()
    {
        var top10 = await GetTopRankings(10); // ���� 10�� ��ŷ �ҷ�����
        DisplayRankings(top10);
    }
}