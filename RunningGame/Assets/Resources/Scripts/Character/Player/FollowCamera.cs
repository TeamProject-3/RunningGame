using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.85f;
    public Vector3 offset;
    private float fixY=-2;

    void Start()
    {
      
        fixY = transform.position.y;
    }

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x , fixY, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
