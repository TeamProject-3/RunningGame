using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    
    [SerializeField]private GameObject[] backGround;
    private List<GameObject> obstaclPrefabs;
   
    bool mapCheck = false;
    bool progressMaxCheck = false;

    float mapfixWidth = 17.92f;

    
    MapManager mapManager;

    private void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        backGround = mapManager.GetstageBackGrounds();
        obstaclPrefabs = mapManager.GetObstaclePrefabs();
        obstaclPrefabs.RemoveAll(mapManager => mapManager.name == "Obstacle_00");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("BackGround"))
        {

            mapCheck = CountMapSetting();
            progressMaxCheck = mapManager.ProgressMaxCheck();
            mapManager.StageNumAddition(mapCheck);

            GameObject check = collision.gameObject;

            int findBackground = -1;

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

            for (int i = 0; i < backGround.Length; i++)
            {
                
                if (i == findBackground || i == nextBackgroundCheck) continue;

                foreach (GameObject obj in obstaclPrefabs)
                {
                    if (obj.activeSelf && obj.transform.position.x + mapManager.fixWidth - mapfixWidth < gameObject.transform.position.x)
                        obj.SetActive(false);

                    if (obj.activeSelf && obj.name == "Obstacle_07")
                        mapManager.StageNumSubtraction(mapCheck);
                }
                    
                foreach (GameObject obj in obstaclPrefabs)
                {
                    
                    if (!obj.activeSelf)
                    {
                        if (!progressMaxCheck && obj.name == "Obstacle_07") continue;

                        if (progressMaxCheck && obj.name != "Obstacle_07") continue;
                        else if (progressMaxCheck && obj.name == "Obstacle_07")
                            mapManager.ProgressMaxCheckFalse();

                        obj.SetActive(true);
                        if (mapCheck)
                        {
                            obj.transform.position = 
                                backGround[3].transform.position +
                                new Vector3(mapfixWidth, 0f, 0f); ;
                        }
                        else
                        {
                            obj.transform.position =
                                backGround[findBackground].transform.position +
                                 new Vector3(mapManager.fixWidth, 0f, 0f);
                        }
                        
                        break;
                    }
                }

                if (mapCheck)
                {
                    for (int j = 0; j < 4; j++)
                        backGround[j].transform.position =
                            backGround[3].transform.position +
                            new Vector3(mapfixWidth*j, 0f, 0f);
                    mapCheck = false;
                    break;
                }
                else
                {
                    backGround[i].transform.position =
                        backGround[nextBackgroundCheck].transform.position +
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

    private bool CountMapSetting()
    {
        if (!(Mathf.Abs(mapfixWidth - mapManager.fixWidth) < 0.001f))
        {
            return true;
        }
        return false;
    }
}
