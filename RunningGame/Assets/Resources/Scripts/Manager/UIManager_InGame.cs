using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager_InGame : MonoBehaviour, IUiUpdate_InGame, IOnButton_InGame, IUiShow_InGame
{
    public GameObject[] controlGameUI;
    //0 - StopButton, 1 - ProgressSlider, 2 - Coin, 3 - Bean, 4 - MapName, 5 - HighScore, 6 - MyScore, 7 - PauseMenu
    // 8 - ResultMenu
    public Text coinText;
    public int coinCount = 0;
    public Text starText;
    public int starCount = 0;
    public Slider progressSlider;
    public int highScore = 0;
    public int myScore = 0;
    public Text highScoreText;
    public Text myScorelText;
    public Text mapName;
    public string mapNameText = "Default Map";

    public static UIManager_InGame Instance { get; private set; }

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

    public void InitializeUI()
    {
        for (int i = 0; i < controlGameUI.Length; i++)
        {
            controlGameUI[i].SetActive(true);
        }
    }


    public void ShowUI(int menu)
    {
        controlGameUI[menu].SetActive(true);
    }

    public void HideUI(int menu)
    {
        controlGameUI[menu].SetActive(false);
    }

    public void HideUI(GameObject UI)
    {
    }
    public void UpdateCoinText()
    {
        Debug.Log("Coin Updated: " + coinCount);
        coinText.text = coinCount.ToString();
    }
    public void UpdateStarText()
    {
        Debug.Log("Star Updated: " + starCount);
        starText.text = starCount.ToString();
    }
    public void UpdateProgressSlider(float value)
    {
        Debug.Log("Progress Updated: " + value);
        progressSlider.value = value;
    }
    public void UpdateHighScoreText()
    {
        Debug.Log("High Score Updated: " + highScore);
        highScoreText.text = highScore.ToString();
    }
    public void UpdateMyScoreText()
    {
        Debug.Log("My Score Updated: " + myScore);
        myScorelText.text = myScore.ToString();
    }
    public void UpdateMapNameText()
    {
        Debug.Log("Map Name Updated: " + mapNameText);
        mapName.text = mapNameText;
    }

    public void OnPauseButton()
    {
        ShowUI(7);
    }
    public void OnContinueButton()
    {
        HideUI(7);
        //타이머 3초 후 게임시작 하도록 로직 필요
    }
    public void OnRestartButton()
    {
        SceneManager.LoadScene("TestScene 4_InGame");//게임 재시작 로직 변경필요}
    }
    public void OnExitButton()
    {
        SceneManager.LoadScene("TestScene 4_Main");
    }
    public void ShowResultUI()
    {
        ShowUI(8);
    }
}