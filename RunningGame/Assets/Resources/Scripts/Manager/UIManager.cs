using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IUiShow, IUiUpdate,IOnButton
{
    public GameObject[] controlUI;
    //0 - TopMenu, 1 - LeftMenu, 2 - BottomMenu, 3 - RightMenu, 4 - PlayerMenu, 5 - RightTopMenu
    public Text diaText;
    public int diamondCount = 0;
    public Text coinText;
    public int coinCount = 0;
    public Slider expSlider;
    public string playername = "Player";
    public Text playerNameText;
    public Text levelText;
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
            controlUI[i].SetActive(true);
        }
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
    public void HamburgerButton()
    {
    }


}
