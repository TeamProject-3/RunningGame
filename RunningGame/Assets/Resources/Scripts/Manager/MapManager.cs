using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private string stageBackGround = "Prefab\\Obstacle\\StageSetting";

    private string stage_One = "Prefab\\Obstacle\\Stage01";
    private string stage_Two = "Prefab\\Obstacle\\Stage02";

    [SerializeField]
    private int stageSelectNum = 1;
    
    protected GameObject[] obstaclePrefabs;
    protected GameObject[] stageBackGrounds;

    private List<GameObject> obstacles = new List<GameObject>();
    private List<GameObject> backObjects = new List<GameObject>();

    private int MaxStageNum = 4;
    [SerializeField]
    private int stageNum = 0;

    private bool progressMax = false;

    [HideInInspector] public float fixWidth = 0;

    MapManager grid;
    UIManager_InGame ui_Ingame;

    private void Awake()
    {
        grid = FindObjectOfType<MapManager>();

        ui_Ingame = UIManager_InGame.Instance;

        stageBackGrounds = Resources.LoadAll<GameObject>(stageBackGround);
        stageSelectNum = DataManager.Instance.crrentDungeon;
        //스테이지 변경
        StageSelect(stageSelectNum);
    }

    private void FixedUpdate()
    {

        ui_Ingame.UpdateProgressSlider(stageNum <= 0 ? 0 : (float)stageNum / (float)MaxStageNum);

        if (stageNum / MaxStageNum == 1) stageNum = 0;
        else if (stageNum == 3) progressMax = true;

    }

    public List<GameObject> GetObstaclePrefabs()
    {
        foreach (GameObject prefab in obstaclePrefabs)
        {
            GameObject instance = Instantiate(prefab, grid.transform);
            instance.name = prefab.name;
            instance.SetActive(instance.name == "Obstacle_00" ? true : false);
            obstacles.Add(instance);
        }

        return obstacles;
    }

    public GameObject[] GetstageBackGrounds()
    {
        
        GameObject background = GameObject.Find("BackGround");

        if (background != null)
        {
            foreach (Transform child in background.transform)
            {
                backObjects.Add(child.gameObject);
            }
        }

        return backObjects.ToArray();
    }

    private void StageSelect(int selectNum)
    {
        GameObject instance;
        switch (selectNum)
        {
            case 1:
                
                obstaclePrefabs = Resources.LoadAll<GameObject>(stage_One);
                instance = Instantiate(stageBackGrounds[0], grid.transform);
                fixWidth = 53.76f;
                break;
            case 2:
                obstaclePrefabs = Resources.LoadAll<GameObject>(stage_Two);
                instance = Instantiate(stageBackGrounds[1], grid.transform);
                fixWidth = 17.92f;
                break;
            default:
                obstaclePrefabs = null;
                break;
        }
    }

    public void StageNumAddition(bool checks)
    {
        stageNum += checks ? 3 : 1;
    }

    public void StageNumSubtraction(bool checks)
    {
        stageNum -= checks ? 3 : 1;
    }

    public bool ProgressMaxCheck()
    {
        return progressMax;
    }

    public void ProgressMaxCheckFalse()
    {
        progressMax = false;
    }
}
