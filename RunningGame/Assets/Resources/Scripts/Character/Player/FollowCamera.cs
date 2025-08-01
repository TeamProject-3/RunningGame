using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

   
    public Transform player;           // 플레이어 트랜스폼
    public float followSpeed = 5f;     // 카메라 따라가는 속도
    public Vector2 screenOffset = new Vector2(0.18f, 0.28f); // 화면에서 플레이어 위치(좌측 하단)
    public float cameraFixedY = 0f;  // 카메라 y축 고정값


    private bool isFollowing = false;  // 따라가기 시작 여부


    private void Start()
    {
       player = FindObjectOfType<Player>().transform; // 플레이어 트랜스폼을 찾기
    }
    void FixedUpdate()
    {
     
        Camera cam = Camera.main;

        // 아직 따라가지 않았다면, 화면에 들어왔는지 확인
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
            // 플레이어를 화면의 원하는 지점(screenOffset)에 고정
            Vector3 targetViewport = new Vector3(screenOffset.x, screenOffset.y, cam.WorldToScreenPoint(player.position).z);
            Vector3 cameraTargetWorld = cam.ViewportToWorldPoint(targetViewport);
            float targetX = player.position.x - (cameraTargetWorld.x - transform.position.x);

            Vector3 desiredPos = new Vector3(targetX, cameraFixedY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);
        }
        else
        {
            // 카메라 Y축 고정
            Vector3 fixedPos = transform.position;
            fixedPos.y = cameraFixedY;
            transform.position = fixedPos;
        }

  
    }
}
