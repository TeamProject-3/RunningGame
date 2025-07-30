using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour, IUiShow, IUiUpdate, IOnButton
{
    public GameObject[] controlUI;
    //0 - MainMenu, 1 - StageMenu, 2 - StartMenu, 3- ShopMenu
    public Text diaText;
    public int diamondCount = 0;
    public Text coinText;
    public int coinCount = 0;
    public Slider expSlider;
    public string playername = "Player";
    public Text playerNameText;
    public Text levelText;
    public Text stageName;
    public Text[] highScoretext;
    public GameObject stageImage;
    public Sprite[] stageImages;
    public int stageIndex = 0;
    void Start()
    {
        InitializeUI(); // 테스트용
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeUI()
    {
        for (int i = 0; i < controlUI.Length; i++)
        {
            HideUI(i);
        }
        ShowUI(0); 
    }

    public void ShowUI(int menu)
    {
        controlUI[menu].SetActive(true);
    }

    public void HideUI(int menu)
    {
        controlUI[menu].SetActive(false);
    }

    public void UpdateDiamont()
    {
        diaText.text = diamondCount.ToString();
    }
    public void UpdateCoin()
    {
        coinText.text = coinCount.ToString();
    }
    public void UpdateExp()
    {
        Debug.Log("Exp Updated: " + expSlider.value);
    }
    public void UpdatePlayerName()
    {
        playerNameText.text = playername;
    }

    public void OnShopButton()
    {
        HideUI(0); 
        ShowUI(3); 
    }
    public void OnGachaButton()
    {
    }

    public void OnAlbum()
    {
    }

    public void OnSettingButton()
    {
    }
    public void OnPlayButton()
    {
    }
    public void OnSaveButton()
    {
    }
    public void OnHamburgerButton()
    {
    }

    public void OnStartButton()
    {
        HideUI(0);
        ShowUI(1);
    }
    public void OnStageSelectButton()
    {
        //스테이지 인덱스를 받는 로직이 필요하다
        HideUI(1);
        ShowUI(2);
        UpdateHighScore();
    }
    public void OnNextStageButton()
    {
        if(stageIndex>=4) // 스테이지 인덱스가 5보다 크면 초기화
        {
            stageIndex = 0;
        }
        else
        {
            stageIndex++; // 다음 스테이지로 이동
        }
        stageImage.GetComponent<Image>().sprite = stageImages[stageIndex]; // 다음 스테이지 이미지로 변경

    }
    public void OnPrevStageButton()
    {
        if (stageIndex <= 0) // 스테이지 인덱스가 5보다 크면 초기화
        {
            stageIndex = 4;
        }
        else
        {
            stageIndex--; // 다음 스테이지로 이동
        }
        stageImage.GetComponent<Image>().sprite = stageImages[stageIndex]; // 다음 스테이지 이미지로 변경

    }
    public void OnRunButton()
    {
        SceneManager.LoadScene("TestScene 4_InGame");//추후변경필요

    }
    public void UpdateHighScore()
    {
        for (int i = 0; i < highScoretext.Length; i++)
        {
            //highScoretext[i].text = 하이스코어 변수값을 받는 로직 필요
        }
    }
    public void OnOutShopButton()
    {
        HideUI(3);
        ShowUI(0);
    }
}