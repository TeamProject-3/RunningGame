using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager_InGame : MonoBehaviour, IUiUpdate_InGame, IOnButton_InGame, IUiShow_InGame
{
    public GameObject[] controlGameUI;
    //0 - StopButton, 1 - ProgressSlider, 2 - Coin, 3 - Bean, 4 - MapName, 5 - HighScore, 6 - MyScore, 7 - PauseMenu
    // 8 - ResultMenu
    public Text coinText;
    public int coinCount = 0;
    public int starCount = 0;
    public Slider progressSlider;
    public Slider hpSlider;
    public int highScore = 0;
    public int myScore = 0;
    public Text exitScoreText;
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
        coinText.text = string.Format("{0:N0}", coinCount);
    }
    
    public void UpdateProgressSlider(float value)
    {
        //Debug.Log("Progress Updated: " + value);
        progressSlider.value = value;
    }

    public void UpdateHpSlider(float value)
    {
        //Debug.Log("Progress Updated: " + value);
        hpSlider.value = value;
    }
    public void UpdateHighScoreText()
    {
        //Debug.Log("High Score Updated: " + highScore);
        highScoreText.text = string.Format("{0:N0}", highScore);
    }
    
    public void UpdateMyScoreText()
    {
        //Debug.Log("My Score Updated: " + myScore);
        myScorelText.text = string.Format("{0:N0}", myScore);
    }
    public void UpdateMapNameText()
    {
        Debug.Log("Map Name Updated: " + mapNameText);
        mapName.text = mapNameText;
    }

    public void ExitScoreText()
    {
        exitScoreText.text = string.Format("{0:N0}", myScore);
    }

    public void OnPauseButton()
    {
        ShowUI(7);
        Time.timeScale = 0f; // 게임 일시정지
    }
    public void OnContinueButton()
    {
        HideUI(7);
        //타이머 3초 후 게임시작 하도록 로직 필요
        Time.timeScale = 1f; // 게임 일시정지
    }
    public void OnRestartButton()
    {
        //현재 씬 다시 로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
    public void OnExitButton()
    {
        SceneManager.LoadScene("TestScene 4_Main");
        Time.timeScale = 1f;
    }
    public void ShowResultUI()
    {
        ShowUI(8);
    }
}