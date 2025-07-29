using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] controlUI;
    //0 - Main UI, �߰����� UI�� ���⿡ ����

    void Start()
    {
        ShowUI(0); // �׽�Ʈ��
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
