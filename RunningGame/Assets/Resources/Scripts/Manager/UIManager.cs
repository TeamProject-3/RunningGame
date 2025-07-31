using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour, IUiShow, IUiUpdate, IOnButton
{
    public GameObject[] controlUI;
    //0 - MainMenu, 1 - StageMenu, 2 - StartMenu, 3- ShopMenu, 4 - NameSetMenu
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
    public Text nameBox;
    public GameObject[] shopMenu;
    public Sprite[] characterImages;
    public GameObject mainCharacterImage;

    //0 - Shop, 1 - Gacha, 2 - Album
    public static UIManager Instance { get; private set; }

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
        InitializeUI();
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
        FirstLoginCheeck();
    }

    public void ShowUI(int menu)
    {
        controlUI[menu].SetActive(true);
    }

    public void HideUI(int menu)
    {
        controlUI[menu].SetActive(false);
    }
    public void ShowShopUI(int menu)
    {
        shopMenu[menu].SetActive(true);
    }
    public void HideShopUI(int menu)
    {
        shopMenu[menu].SetActive(false);
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
        playerNameText.text = DataManager.Instance.currentPlayerdata.userName;
    }

    public void OnShopButton()
    {
        HideUI(0); 
        ShowUI(3); 
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
    public void OnNameCheckButton()
    {
        DataManager.Instance.SetName(nameBox.text);
        UpdatePlayerName();
        HideUI(4);
    }
    public void ShowNameSetMenu()
    {
        ShowUI(4);
    }

    public void FirstLoginCheeck()
    {
        if (!DataManager.Instance.currentPlayerdata.isSetName)
        {
            ShowNameSetMenu();
        }
    }
    public void OnBuyButton()
    {
        ShowShopUI(0);
        HideShopUI(1);
        HideShopUI(2);
    }
    public void OnGachaButton()
    {
        ShowShopUI(1);
        HideShopUI(0);
        HideShopUI(2);
    }

    public void OnAlbum()
    {
        ShowShopUI(2);
        HideShopUI(0);
        HideShopUI(1);
    }
    public void OnBuySelectButton()
    {
        // 캐릭터 구매 로직
        // 구매한 캐릭터 스프라이트 변경
        // 다시 클릭 해도 구매하지 않도록 로직필요
        Debug.Log("Character Buy Button Clicked");
    }

    public void OnGachaSelectButton()
    {
        // 확률적으로 캐릭터를 구매할 수 있는 로직
        // 구매한 캐릭터 스프라이트 변경
        // 다시 클릭 해도 구매하지 않도록 로직필요
        Debug.Log("Character Buy Button Clicked");
    }
    public void OnAlbumSelectButton1()
    {
        mainCharacterImage.GetComponent<Image>().sprite = characterImages[0];
        //캐릭터 Data에도 변경하도록 로직필요
    }
    public void OnAlbumSelectButton2()
    {
        mainCharacterImage.GetComponent<Image>().sprite = characterImages[1];
        //캐릭터 Data에도 변경하도록 로직필요
    }
    public void OnAlbumSelectButton3()
    {
        mainCharacterImage.GetComponent<Image>().sprite = characterImages[2];
        //캐릭터 Data에도 변경하도록 로직필요
    }
    public void OnAlbumSelectButton4()
    {
        mainCharacterImage.GetComponent<Image>().sprite = characterImages[3];
        //캐릭터 Data에도 변경하도록 로직필요
    }
}