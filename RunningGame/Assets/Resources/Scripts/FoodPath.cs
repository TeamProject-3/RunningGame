using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FoodPath : MonoBehaviour
{
     [Tooltip("음식 프리팹")]
    public GameObject foodPrefab;

    [Tooltip("음식과 음식의 거리")]
    public float spacing = 0.5f;

    private List<Transform> points;

    // 에디터에서만 보이는 경로 시각화 기능 (오브젝트 선택시)
    private void OnDrawGizmos()
    {
        // 실시간으로 자식 오브젝트를 가져옴
        var editorPoints = GetComponentsInChildren<Transform>().ToList();
        editorPoints.Remove(transform); // 자기 자신은 제외

        if (editorPoints.Count < 2) return;

        for (int i = 0; i < editorPoints.Count - 1; i++)
        {
            // 현재 점부터 다음 점 위치
            Vector3 worldPoint1 = editorPoints[i].position;
            Vector3 worldPoint2 = editorPoints[i + 1].position;

            Gizmos.color = Color.red; // 색상을 바꿔 구별
            Gizmos.DrawLine(worldPoint1, worldPoint2);
        }
    }

    void Awake()
    {
        points = new List<Transform>();

        // 자신의 바로 아래 자식들을 순서대로 points 리스트에 추가
        foreach (Transform child in transform)
        {
            points.Add(child);
        }
    }

    void Start()
    {
        if (foodPrefab == null) return;
        
        GenerateJellies();
        
    }


    // 음식 생성
    void GenerateJellies()
    {
        if (points == null || points.Count < 2)
        {
            Debug.Log("젤리 생성 완료");
            return;
        }

        // 경로 첫번째 점에 음식 생성
        Instantiate(foodPrefab, points[0].position, Quaternion.identity, transform);

        // 경로 전체에 걸쳐 누적된 거리
        float distanceSinceLastFood = 0f;

        // 경로의 모든 선을 순회
        for (int i = 0; i < points.Count - 1; i++)
        {
            // 각 선의 시작점과 끝점 (월드 좌표)
            Vector3 startPoint = points[i].position;
            Vector3 endPoint = points[i + 1].position;

            // 선의 방향과 길이 계산
            Vector3 direction = (endPoint - startPoint).normalized; //normalized 정규화된 방향 백터 
            float distance = Vector3.Distance(startPoint, endPoint); //Distance 거리 측정

            while (distanceSinceLastFood + distance >= spacing)
            {
                // 다음 음식가 놓일 위치까지 필요한 거리
                float distanceToNextFood = spacing - distanceSinceLastFood;

                // 다음 음식의 위치를 계산
                Vector3 FoodPosition = startPoint + direction * distanceToNextFood; 
                Instantiate(foodPrefab, FoodPosition, Quaternion.identity, transform);

                // 현재 선의 남은 거리를 계산하고, 누적 거리를 0으로 초기화
                distance -= distanceToNextFood;
                startPoint = FoodPosition; // 다음 계산을 위해 시작점을 현재 음식 위치로 옮김
                distanceSinceLastFood = 0f;
            }

            // 현재 선분을 다 지나고도 다음 음식를 놓을 만큼의 거리가 되지 않으면, 남은 거리를 다음 선분 계산을 위해 누적
            distanceSinceLastFood += distance;
        }
    }
}
