using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Main : MonoBehaviour
{
    public static GameManager_Main Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
