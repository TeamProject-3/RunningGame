using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] controlUI;
    //0 - Main UI, 추가적인 UI는 여기에 적기

    void Start()
    {
        ShowUI(0); // 테스트용
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeUI()
    {
        for (int i = 0; i < controlUI.Length; i++)
        {
            controlUI[i].SetActive(false);
        }
    }

    public void ShowUI(int menu)
    {
        controlUI[menu].SetActive(true);
    }

    public void OnUIButton()
    {
    }
}
