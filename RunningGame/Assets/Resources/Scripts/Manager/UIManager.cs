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
    public int[] itemValue; // ������ ���� ������ ����
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
        // �������� �̸� ������Ʈ
        DataManager.Instance.currentDungeon = stageIndex + 1;
    }

    public void OnNextStageButton()
    {
        if(stageIndex>=4) // �������� �ε����� 5���� ũ�� �ʱ�ȭ
        {
            stageIndex = 0;
        }
        else
        {
            stageIndex++; // ���� ���������� �̵�
        }
        stageImage.GetComponent<Image>().sprite = stageImages[stageIndex]; // ���� �������� �̹����� ����

    }
    public void OnPrevStageButton()
    {
        if (stageIndex <= 0) // �������� �ε����� 5���� ũ�� �ʱ�ȭ
        {
            stageIndex = 4;
        }
        else
        {
            stageIndex--; // ���� ���������� �̵�
        }
        stageImage.GetComponent<Image>().sprite = stageImages[stageIndex]; // ���� �������� �̹����� ����

    }
    public void OnRunButton()
    {
        SceneManager.LoadScene("InGameScene");//���ĺ����ʿ�
    }
    public void UpdateHighScore()
    {
        stageName.text =(DataManager.Instance.currentDungeon).ToString() + " ��������";
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
            if (c >= 0xAC00 && c <= 0xD7A3) // �ѱ� �����ڵ� ����
            {
                hasKorean = true;
                break;
            }
        }

        int maxLength = hasKorean ? 7 : 12;

        if (input.Length > maxLength)
        {
            alertText[0].text = "�̸��� �ѱ� 7����, ���� 12���� ���Ϸ� �Է����ּ���.";
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
        // ����Ʈ�� �ش� ĳ���Ͱ� �ִ��� Ȯ��
        return DataManager.Instance.currentPlayerdata.characters.Contains(characterType);
    }

    public void OnBuySelectButton()
    {
        if (CheckCharacter("PlayerOrcy"))
        {
            alertText[1].text = "�̹� ������ ĳ���� �Դϴ�.";
        }
        else
        {
            if (DataManager.Instance.currentPlayerdata.gold < itemValue[0])
            {
                alertText[1].text = "��尡 �����մϴ�.";
                alertText[1].gameObject.SetActive(true);
                return;
            }
            else
            {
                alertText[1].text = "���Ű� �Ϸ����ϴ�.";
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
            alertText[2].text = "�̹� ������ ĳ���� �Դϴ�.";
        }
        else
        {
            if (DataManager.Instance.currentPlayerdata.gold < itemValue[1])
            {
                alertText[2].text = "��尡 �����մϴ�.";
                alertText[2].gameObject.SetActive(true);
                return;
            }
            else
            {
                DataManager.Instance.currentPlayerdata.gold -= itemValue[1];
                UpdateCoin();
                if (Random.Range(0, 319) == 0) // % Ȯ���� ���� ����
                {
                    alertText[2].text = "��í�� �����߽��ϴ�.";
                    DataManager.Instance.SetCharacter("PlayerSharky");
                    return;
                }
                else
                {
                    alertText[2].text = "��í�� �����߽��ϴ�.";
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
    //            Debug.LogWarning("�� �� ���� ĳ���� Ÿ���Դϴ�.");
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
            Debug.Log("�̹� ���õ� ĳ�����Դϴ�: " + (CharacterType)characterIndex);
            // �̹� ���õ� ĳ���Ͷ�� ���� UI????
            return;
        }
        else
        {
            // DataManager.Instance.SetCurrentCharacter(characterIndex);
            DataManager.Instance.SetCurrentCharacter((CharacterType)characterIndex);


            Debug.Log("ĳ���� �����: " + (CharacterType)characterIndex);

            // ĳ���� ���� UI ������Ʈ
            // GameManager.Instance.ChangeCharacterImage();
            ChangeCharacterImage();
            DataManager.Instance.OnSaveData();
        }
    }


    [SerializeField]
    private GameObject changeCharacters;

    public void ChangeCharacterImage()
    {
        // �ڽ� ������Ʈ ������ 
        Transform[] gameObjects = changeCharacters.GetComponentsInChildren<Transform>(true);


        foreach (Transform t in gameObjects)
        {
            // �θ� ������Ʈ�� �ǳʶ�
            if (t == changeCharacters.transform)
                continue;

            // ĳ���� �̸��� ���� �÷��̾� �������� ĳ���� �̸� ��
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
        // ������ ĳ���Ϳ� �ش��ϴ� ��ư�� Ȱ��ȭ
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
                    Debug.LogWarning("�� �� ���� ĳ���� Ÿ���Դϴ�.");
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
        DataManager.Instance.currentPlayerdata.gold += 1000; // ���÷� 1000��� �߰�
        UpdateCoin();
    }
}