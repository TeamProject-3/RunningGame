using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    private string stageBackGround = "Prefab\\Obstacle\\StageSetting";

    private string stage_One = "Prefab\\Obstacle\\Stage01";
    private string stage_Two = "Prefab\\Obstacle\\Stage02";

    [SerializeField]
    private int stageSelectNum = 1;
    
    protected GameObject[] obstaclePrefabs;
    protected GameObject[] stageBackGrounds;

    private List<GameObject> obstacles = new List<GameObject>();
    private List<GameObject> backObjects = new List<GameObject>();
    private GameObject empthyBackObjects;

    // �ִ� ��������
    private int MaxStageNum = 2;

    public int stageNum = 0;


    [HideInInspector] public float fixWidth = 17.92f;

    [HideInInspector] public float totalWidth = 0;
    [HideInInspector] public float totalMapLength;

    [HideInInspector] public int loopPoint = 0;

    //[HideInInspector] 
    public bool mapCheck = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        stageBackGrounds = Resources.LoadAll<GameObject>(stageBackGround);
        stageSelectNum = DataManager.Instance.currentDungeon;

        //�������� ����
        StageSelect(stageSelectNum);

        totalMapLength = totalWidth * MaxStageNum;
    }



    public List<GameObject> GetObstaclePrefabs()
    {
        foreach (GameObject prefab in obstaclePrefabs)
        {
            GameObject instance = Instantiate(prefab, gameObject.transform);
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
    public GameObject GetstageEmpthyBackGrounds()
    {

        GameObject background = GameObject.Find("EmpthyBackGround");

        if (background != null)
            empthyBackObjects = background;

        return empthyBackObjects;
    }

    private void StageSelect(int selectNum)
    {
        GameObject instance;
        switch (selectNum)
        {
            case 1:
                
                obstaclePrefabs = Resources.LoadAll<GameObject>(stage_One);
                instance = Instantiate(stageBackGrounds[0], gameObject.transform);
                totalWidth = fixWidth * 3;
                break;
            case 2:
                obstaclePrefabs = Resources.LoadAll<GameObject>(stage_Two);
                instance = Instantiate(stageBackGrounds[1], gameObject.transform);
                totalWidth = fixWidth ;
                break;
            default:
                obstaclePrefabs = null;
                break;
        }
    }

    public void StageNumAdd()
    {
        stageNum++;
    }


    public void MapCheck()
    {
        if (stageNum >= MaxStageNum)
        {
            mapCheck = true;
            stageNum = 0;
        }
    }

}
