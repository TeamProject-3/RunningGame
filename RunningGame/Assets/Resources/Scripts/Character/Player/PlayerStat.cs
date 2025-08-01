using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Range(0, 10)][SerializeField] private float hp = 10;
    public float Hp
    {
        get { return hp; }
        set
        {
            hp = Mathf.Clamp(value, 0, 10);
        }
    }


    public float jumpForce = 7f;
    public float slideSpeed = 5f;
    public float moveSpeed = 5f;
    public float currentGravityScale;

    // ¼öÁ¤ X
    public readonly float baseMoveSpeed = 6.75f;
    public readonly float baseJumpForce = 22f;
    public readonly float baseGravityScale = 7f;
}
