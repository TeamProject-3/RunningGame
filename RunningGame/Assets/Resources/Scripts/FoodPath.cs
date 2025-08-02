using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FoodPath : MonoBehaviour
{
     [Tooltip("���� ������")]
    public GameObject foodPrefab;

    [Tooltip("���İ� ������ �Ÿ�")]
    public float spacing = 0.5f;

    private List<Transform> points;

    // �����Ϳ����� ���̴� ��� �ð�ȭ ��� (������Ʈ ���ý�)
    private void OnDrawGizmos()
    {
        // �ǽð����� �ڽ� ������Ʈ�� ������
        var editorPoints = GetComponentsInChildren<Transform>().ToList();
        editorPoints.Remove(transform); // �ڱ� �ڽ��� ����

        if (editorPoints.Count < 2) return;

        for (int i = 0; i < editorPoints.Count - 1; i++)
        {
            // ���� ������ ���� �� ��ġ
            Vector3 worldPoint1 = editorPoints[i].position;
            Vector3 worldPoint2 = editorPoints[i + 1].position;

            Gizmos.color = Color.red; // ������ �ٲ� ����
            Gizmos.DrawLine(worldPoint1, worldPoint2);
        }
    }

    void Awake()
    {
        points = new List<Transform>();

        // �ڽ��� �ٷ� �Ʒ� �ڽĵ��� ������� points ����Ʈ�� �߰�
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


    // ���� ����
    void GenerateJellies()
    {
        if (points == null || points.Count < 2)
        {
            Debug.Log("���� ���� �Ϸ�");
            return;
        }

        // ��� ù��° ���� ���� ����
        Instantiate(foodPrefab, points[0].position, Quaternion.identity, transform);

        // ��� ��ü�� ���� ������ �Ÿ�
        float distanceSinceLastFood = 0f;

        // ����� ��� ���� ��ȸ
        for (int i = 0; i < points.Count - 1; i++)
        {
            // �� ���� �������� ���� (���� ��ǥ)
            Vector3 startPoint = points[i].position;
            Vector3 endPoint = points[i + 1].position;

            // ���� ����� ���� ���
            Vector3 direction = (endPoint - startPoint).normalized; //normalized ����ȭ�� ���� ���� 
            float distance = Vector3.Distance(startPoint, endPoint); //Distance �Ÿ� ����

            while (distanceSinceLastFood + distance >= spacing)
            {
                // ���� ���İ� ���� ��ġ���� �ʿ��� �Ÿ�
                float distanceToNextFood = spacing - distanceSinceLastFood;

                // ���� ������ ��ġ�� ���
                Vector3 FoodPosition = startPoint + direction * distanceToNextFood; 
                Instantiate(foodPrefab, FoodPosition, Quaternion.identity, transform);

                // ���� ���� ���� �Ÿ��� ����ϰ�, ���� �Ÿ��� 0���� �ʱ�ȭ
                distance -= distanceToNextFood;
                startPoint = FoodPosition; // ���� ����� ���� �������� ���� ���� ��ġ�� �ű�
                distanceSinceLastFood = 0f;
            }

            // ���� ������ �� ������ ���� ���ĸ� ���� ��ŭ�� �Ÿ��� ���� ������, ���� �Ÿ��� ���� ���� ����� ���� ����
            distanceSinceLastFood += distance;
        }
    }
}
