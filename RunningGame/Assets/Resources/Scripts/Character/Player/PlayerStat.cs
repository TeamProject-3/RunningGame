using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Range(0, 100)][SerializeField] private float hp = 10;
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
}
