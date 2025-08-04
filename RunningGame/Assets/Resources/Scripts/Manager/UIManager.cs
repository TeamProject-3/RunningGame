using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;


public class UIManager : MonoBehaviour, IUiShow, IUiUpdate, IOnButton
{
    public GameObject[] controlUI;
    //0 - MainMenu, 1 - StageMenu, 2 - StartMenu, 3- ShopMenu, 4 - NameSetMenu, 5 - CharacterSprite
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
    Image characterImage;
    public Text[] alertText;
    //0 - name, 1 - gold
    public GameObject[] characterButtons;
    //0 - PlayerFishy, 1 - PlayerOrcy, 2 - PlayerPescy, 3 - PlayerSharky
    public int[] itemValue; // 아이템 가격 설정용 변수
    // 0 - Shop, 1 - Gacha

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
        SetComponent();
        for (int i = 0; i < controlUI.Length; i++)
        {
            HideUI(i);
        }
        ShowUI(0);
        FirstLoginCheeck();
        ChangeCharacterImage();
        //UpdatePlayerSprite();
        ShowUI(5);
        InitCharacterUI();
        UpdateCoin();
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
        coinText.text = DataManager.Instance.currentPlayerdata.gold.ToString();
    }
    public void UpdateExp()
    {
        Debug.Log("Exp Updated: " + expSlider.value);
    }
    public void UpdatePlayerName()
    {
        playerNameText.text = DataManager.Instance.currentPlayerdata.userName;
        DataManager.Instance.OnSaveData();
    }

    public void OnShopButton()
    {
        HideUI(0);
        HideUI(5);
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
        HideUI(5);
        ShowUI(1);
    }
    public void OnStageSelectButton()
    {
        if (stageIndex < 2)
        {
            HideUI(1);
            ShowUI(2);
            UpdateMapData();
            UpdateHighScore();
        }
    }
    public void UpdateMapData()
    {
        // 스테이지 이름 업데이트
        DataManager.Instance.currentDungeon = stageIndex + 1;
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
        SceneManager.LoadScene("InGameScene");//추후변경필요
    }
    public void UpdateHighScore()
    {
        stageName.text =(DataManager.Instance.currentDungeon).ToString() + " 스테이지";
        RankingManager RM = this.GetComponent<RankingManager>();

        RM.ShowRanking(DataManager.Instance.currentDungeon);
    }
    public void OnOutShopButton()
    {
        HideUI(3);
        ShowUI(5);
        ShowUI(0);
    }
    public void OnNameCheckButton()
    {
        string input = nameBox.text;
        bool hasKorean = false;

        foreach (char c in input)
        {
            if (c >= 0xAC00 && c <= 0xD7A3) // 한글 유니코드 범위
            {
                hasKorean = true;
                break;
            }
        }

        int maxLength = hasKorean ? 7 : 12;

        if (input.Length > maxLength)
        {
            alertText[0].text = "이름은 한글 7글자, 영어 12글자 이하로 입력해주세요.";
            alertText[0].gameObject.SetActive(true);
            nameBox.text = input.Substring(0, maxLength);
            return;
        }
        else
        {
            alertText[0].gameObject.SetActive(false);
        }
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
        else
        {
            UpdatePlayerName();
        }
    }
    public void OnBuyButton()
    {
        ShowShopUI(0);
        HideUI(5);
        HideShopUI(1);
        HideShopUI(2);
    }
    public void OnGachaButton()
    {
        ShowShopUI(1);
        HideUI(5);
        HideShopUI(0);
        HideShopUI(2);
    }

    public void OnAlbum()
    {
        ShowShopUI(2);
        ShowUI(5);
        HideShopUI(0);
        HideShopUI(1);
        UpdatdCharacterImage();
    }
    public bool CheckCharacter(string name)
    {
        if (!System.Enum.TryParse<CharacterType>(name, out var characterType))
        {
            return false;
        }
        // 리스트에 해당 캐릭터가 있는지 확인
        return DataManager.Instance.currentPlayerdata.characters.Contains(characterType);
    }

    public void OnBuySelectButton()
    {
        if (CheckCharacter("PlayerOrcy"))
        {
            alertText[1].text = "이미 구매한 캐릭터 입니다.";
        }
        else
        {
            if (DataManager.Instance.currentPlayerdata.gold < itemValue[0])
            {
                alertText[1].text = "골드가 부족합니다.";
                alertText[1].gameObject.SetActive(true);
                return;
            }
            else
            {
                alertText[1].text = "구매가 완료됬습니다.";
                DataManager.Instance.currentPlayerdata.gold -= itemValue[0];
                DataManager.Instance.SetCharacter("PlayerOrcy");
                UpdateCoin();
            }
        }
    }

    public void OnGachaSelectButton()
    {
        if (CheckCharacter("PlayerSharky"))
        {
            alertText[2].text = "이미 구매한 캐릭터 입니다.";
        }
        else
        {
            if (DataManager.Instance.currentPlayerdata.gold < itemValue[1])
            {
                alertText[2].text = "골드가 부족합니다.";
                alertText[2].gameObject.SetActive(true);
                return;
            }
            else
            {
                DataManager.Instance.currentPlayerdata.gold -= itemValue[1];
                UpdateCoin();
                if (Random.Range(0, 319) == 0) // % 확률로 구매 성공
                {
                    alertText[2].text = "가챠에 성공했습니다.";
                    DataManager.Instance.SetCharacter("PlayerSharky");
                    return;
                }
                else
                {
                    alertText[2].text = "가챠에 실패했습니다.";
                    return;
                }
            }
        }
    }
    //public void OnAlbumSelectButton1()
    //{
    //    DataManager.Instance.SetCurrentCharacter(CharacterType.PlayerFishy);
    //    UpdatePlayerSprite();
    //}
    //public void OnAlbumSelectButton2()
    //{
    //    DataManager.Instance.SetCurrentCharacter(CharacterType.PlayerOrcy);
    //    UpdatePlayerSprite();
    //}
    //public void OnAlbumSelectButton3()
    //{
    //    DataManager.Instance.SetCurrentCharacter(CharacterType.PlayerPescy);
    //    UpdatePlayerSprite();
    //}
    //public void OnAlbumSelectButton4()
    //{
    //    DataManager.Instance.SetCurrentCharacter(CharacterType.PlayerSharky);
    //    UpdatePlayerSprite();
    //}
    public void OnBackStageButton()
    {
        HideUI(1);
        ShowUI(5);
        ShowUI(0);
        stageIndex = 0;
        stageImage.GetComponent<Image>().sprite = stageImages[stageIndex];
    }
    public void OnBackStartButton()
    {
        HideUI(2);
        ShowUI(1);
        stageIndex = 0;
        stageImage.GetComponent<Image>().sprite = stageImages[stageIndex];
    }
    //public void UpdatePlayerSprite()
    //{
    //    Transform characterImageTransform = controlUI[5].transform.Find("CharacterImage(dummy)");

    //    switch (DataManager.Instance.currentPlayerdata.currentCharacter)
    //    {
    //        case CharacterType.PlayerFishy:
    //            characterImage.sprite = characterImages[0];
    //            break;
    //        case CharacterType.PlayerOrcy:
    //            characterImage.sprite = characterImages[1];
    //            break;
    //        case CharacterType.PlayerPescy:
    //            characterImage.sprite = characterImages[2];
    //            break;
    //        case CharacterType.PlayerSharky:
    //            characterImage.sprite = characterImages[3];
    //            break;
    //        default:
    //            Debug.LogWarning("알 수 없는 캐릭터 타입입니다.");
    //            break;
    //    }
    //}
    public void SetComponent()
    {
        Transform characterImageTransform = controlUI[5].transform.Find("CharacterImage(dummy)");
        characterImage = characterImageTransform.GetComponent<Image>();
    }


    public void ChangeCharacter(int characterIndex)
    {
        if (DataManager.Instance.currentPlayerdata.currentCharacter == (CharacterType)characterIndex)
        {
            Debug.Log("이미 선택된 캐릭터입니다: " + (CharacterType)characterIndex);
            // 이미 선택된 캐릭터라고 띄우는 UI????
            return;
        }
        else
        {
            // DataManager.Instance.SetCurrentCharacter(characterIndex);
            DataManager.Instance.SetCurrentCharacter((CharacterType)characterIndex);


            Debug.Log("캐릭터 변경됨: " + (CharacterType)characterIndex);

            // 캐릭터 변경 UI 업데이트
            // GameManager.Instance.ChangeCharacterImage();
            ChangeCharacterImage();
            DataManager.Instance.OnSaveData();
        }
    }


    [SerializeField]
    private GameObject changeCharacters;

    public void ChangeCharacterImage()
    {
        // 자식 오브젝트 가져옴 
        Transform[] gameObjects = changeCharacters.GetComponentsInChildren<Transform>(true);


        foreach (Transform t in gameObjects)
        {
            // 부모 오브젝트는 건너뜀
            if (t == changeCharacters.transform)
                continue;

            // 캐릭터 이름과 현재 플레이어 데이터의 캐릭터 이름 비교
            if (t.name == DataManager.Instance.currentPlayerdata.currentCharacter.ToString())
            {
                t.gameObject.SetActive(true);
            }
            else
            {
                t.gameObject.SetActive(false);
            }
        }
    }
    public void UpdatdCharacterImage()
    {
        // 보유한 캐릭터에 해당하는 버튼만 활성화
        foreach (var character in DataManager.Instance.currentPlayerdata.characters)
        {
            int idx = -1;
            switch (character)
            {
                case CharacterType.PlayerFishy:
                    idx = 0;
                    break;
                case CharacterType.PlayerOrcy:
                    idx = 1;
                    break;
                case CharacterType.PlayerPescy:
                    idx = 2;
                    break;
                case CharacterType.PlayerSharky:
                    idx = 3;
                    break;
                default:
                    Debug.LogWarning("알 수 없는 캐릭터 타입입니다.");
                    break;
            }
            if (idx >= 0 && idx < characterButtons.Length)
            {
                Image btnImage = characterButtons[idx].GetComponent<Image>();
                btnImage.color = Color.white; 
                Button btn = characterButtons[idx].GetComponent<Button>();
                btn.interactable = true;
            }
        }
    }
    public void InitCharacterUI()
    {
        for (int i = 0; i < characterButtons.Length; i++)
        {
            Image btnImage = characterButtons[i].GetComponent<Image>();
            if (btnImage != null)
            {
                btnImage.color = Color.black;
            }

            Button btn = characterButtons[i].GetComponent<Button>();
            if (btn != null)
            {
                btn.interactable = false;
            }
        }
    }

    public void AddGold()
    {
        DataManager.Instance.currentPlayerdata.gold += 1000; // 예시로 1000골드 추가
        UpdateCoin();
    }
}