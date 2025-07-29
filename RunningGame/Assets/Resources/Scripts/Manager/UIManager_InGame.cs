using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_InGame : MonoBehaviour, IUiUpdate_InGame, IOnButton_InGame
{
    public GameObject[] controlGameUI;
    //0 - StopButton, 1 - ProgressSlider, 2 - Coin, 3 - Bean, 4 - MapName, 5 - HighScore, 6 - MyScore
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


    public void InitializeUI()
    {
        for (int i = 0; i < controlGameUI.Length; i++)
        {
            controlGameUI[i].SetActive(true);
        }
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
    public void OnStopButton()
    {
    }
}
