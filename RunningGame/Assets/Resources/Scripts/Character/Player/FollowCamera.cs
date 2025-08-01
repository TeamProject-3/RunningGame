using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

   
    public Transform player;           // �÷��̾� Ʈ������
    public float followSpeed = 5f;     // ī�޶� ���󰡴� �ӵ�
    public Vector2 screenOffset = new Vector2(0.18f, 0.28f); // ȭ�鿡�� �÷��̾� ��ġ(���� �ϴ�)
    public float cameraFixedY = 0f;  // ī�޶� y�� ������


    private bool isFollowing = false;  // ���󰡱� ���� ����


    private void Start()
    {
       player = FindObjectOfType<Player>().transform; // �÷��̾� Ʈ�������� ã��
    }
    void FixedUpdate()
    {
     
        Camera cam = Camera.main;

        // ���� ������ �ʾҴٸ�, ȭ�鿡 ���Դ��� Ȯ��
        if (!isFollowing)
        {
            Vector3 vPos = cam.WorldToViewportPoint(player.position);
            if (vPos.x >= 0 && vPos.x <= 1)
            {
                isFollowing = true;
            }
        }

        if (isFollowing)
        {
            // �÷��̾ ȭ���� ���ϴ� ����(screenOffset)�� ����
            Vector3 targetViewport = new Vector3(screenOffset.x, screenOffset.y, cam.WorldToScreenPoint(player.position).z);
            Vector3 cameraTargetWorld = cam.ViewportToWorldPoint(targetViewport);
            float targetX = player.position.x - (cameraTargetWorld.x - transform.position.x);

            Vector3 desiredPos = new Vector3(targetX, cameraFixedY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);
        }
        else
        {
            // ī�޶� Y�� ����
            Vector3 fixedPos = transform.position;
            fixedPos.y = cameraFixedY;
            transform.position = fixedPos;
        }

  
    }
}
