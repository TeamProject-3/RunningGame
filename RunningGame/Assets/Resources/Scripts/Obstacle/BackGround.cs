using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    
    [SerializeField]private GameObject[] backGround;
    private GameObject empthyBackGround;
    private List<GameObject> obstaclPrefabs;
   
    float mapfixWidth = 0f;

    int findBackground = 0;

    bool healthMapCheck = false;

    private void Start()
    {
        //MapManager에서 맵 길이를 가져옮
        
        backGround = MapManager.Instance.GetstageBackGrounds();
        obstaclPrefabs = MapManager.Instance.GetObstaclePrefabs();
        empthyBackGround = MapManager.Instance.GetstageEmpthyBackGrounds();
        obstaclPrefabs.RemoveAll(mapManager => mapManager.name == "Obstacle_00");
        
        foreach (GameObject obj in obstaclPrefabs)
        {
            Debug.Log("Obstacle Prefab name: " + obj.name);
            obj.SetActive(true);
            obj.transform.position = new Vector3(-900, 80, 0);

            if (obj.name != "Obstacle_07")
                foreach (Transform child in obj.transform.Find("FoodPath"))
                {
                    if (child.name.StartsWith("GameObject"))
                    {
                        Destroy(child.gameObject);
                    }
                }

        }
        mapfixWidth = MapManager.Instance.totalWidth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("BackGround"))
        {
            GameObject check = collision.gameObject;

            if (!(empthyBackGround == check) )
                MapManager.Instance.StageNumAdd();

            MapManager.Instance.MapCheck();

            for (int i = 0; i < backGround.Length; i++)
            {
                if (backGround[i] == check)
                {

                    findBackground = i;
                    break;
                }
            }

            int nextBackgroundCheck = (findBackground + 1) % backGround.Length;

            ObstacleShuffle();

            foreach (GameObject obj in obstaclPrefabs)
            {
                if (obj.activeSelf && obj.transform.position.x < gameObject.transform.position.x)
                    obj.SetActive(false);
            }

            foreach (GameObject obj in obstaclPrefabs)
            {

                if (!MapManager.Instance.mapCheck && obj.name == "Obstacle_07") continue;
                else if (MapManager.Instance.mapCheck && obj.name != "Obstacle_07") continue;

                if (!obj.activeSelf)
                {
                    obj.SetActive(true);


                    if (obj.name != "Obstacle_07")
                        foreach (Transform child in obj.transform.Find("FoodPath"))
                            child.gameObject.SetActive(true);

                    GameObject items = obj.transform.Find("Items").gameObject;
                    if (items != null)
                        foreach (Transform child in items.transform)
                            child.gameObject.SetActive(true);

                    if (MapManager.Instance.mapCheck)
                    {
                        
                        obj.transform.position =
                            backGround[findBackground].transform.position +
                            new Vector3(mapfixWidth, 0f, 0f);
                        healthMapCheck = true;
                    }
                    else
                    {
                        if (healthMapCheck)
                        {
                            obj.transform.position =
                                empthyBackGround.transform.position +
                                new Vector3(MapManager.Instance.fixWidth, 0f, 0f);

                            PlayerStat playerStat = FindObjectOfType<PlayerStat>();
                            InGameManager.Instance.SetSpeed(playerStat.moveSpeed + InGameManager.Instance.speedIncrease);

                            Player player = FindObjectOfType<Player>();
                            player.damage += player.damage / 5;
                           
                        }
                        else
                        {
                            obj.transform.position =
                                 backGround[findBackground].transform.position +
                                 new Vector3(mapfixWidth, 0f, 0f);
                        }
                    }
                    break;
                }
            }

            if (MapManager.Instance.mapCheck)
            {
                empthyBackGround.transform.position =
                    backGround[findBackground].transform.position +
                    new Vector3(mapfixWidth, 0f, 0f);
                MapManager.Instance.mapCheck = false;
            }
            else
            {
                if (healthMapCheck)
                {
                    backGround[nextBackgroundCheck].transform.position =
                                empthyBackGround.transform.position +
                                new Vector3(MapManager.Instance.fixWidth, 0f, 0f);
                    healthMapCheck = false;
                }
                else
                {
                    backGround[nextBackgroundCheck].transform.position =
                        backGround[findBackground].transform.position +
                        new Vector3(mapfixWidth, 0f, 0f);
                }
            }

        }

    }

    private void ObstacleShuffle()
    {
        for (int i = 0; i < obstaclPrefabs.Count; i++)
        {
            int random = Random.Range(i, obstaclPrefabs.Count);

            GameObject obj = obstaclPrefabs[i];
            obstaclPrefabs[i] = obstaclPrefabs[random];
            obstaclPrefabs[random] = obj;
        }
    }


}
