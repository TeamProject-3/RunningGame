using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Range(0, 100)][SerializeField] private float hp = 100;
    public float maxHp;
    public float Hp
    {
        get { return hp; }
        set
        {
            hp = Mathf.Clamp(value, 0, 100);
        }
    }

    public float jumpForce = 22f;
    // public float slideSpeed = 6.75f;
    public float moveSpeed = 6.75f;
    public float currentGravityScale;

    // ¼öÁ¤ X
    public readonly float baseMoveSpeed = 6.75f;
    public readonly float baseJumpForce = 22f;
    public readonly float baseGravityScale = 7f; 

}
