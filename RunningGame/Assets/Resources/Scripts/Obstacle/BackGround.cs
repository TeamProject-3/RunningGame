using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    
    [SerializeField]private GameObject[] backGround;
    private List<GameObject> obstaclPrefabs;
   
    float mapfixWidth = 0f;
    

    private void Start()
    {
        //MapManager에서 맵 길이를 가져옮
        
        backGround = MapManager.Instance.GetstageBackGrounds();
        obstaclPrefabs = MapManager.Instance.GetObstaclePrefabs();
        obstaclPrefabs.RemoveAll(mapManager => mapManager.name == "Obstacle_00");
        mapfixWidth = MapManager.Instance.totalWidth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("BackGround"))
        {
            MapManager.Instance.StageNumAddition();
            MapManager.Instance.MapCheck();

            GameObject check = collision.gameObject;

            int findBackground = 0;

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
                    if (obj.activeSelf && obj.transform.position.x + MapManager.Instance.totalWidth - mapfixWidth < gameObject.transform.position.x)
                        obj.SetActive(false);
                }
                    
                foreach (GameObject obj in obstaclPrefabs)
                {
                    
                    if (!obj.activeSelf)
                    {
                        obj.SetActive(true);
                        if (MapManager.Instance.mapCheck)
                        {

                            break;
                        }
                        obj.transform.position =
                            backGround[findBackground].transform.position +
                             new Vector3(mapfixWidth, 0f, 0f);

                        break;
                    }
                }

                backGround[i].transform.position =
                    backGround[nextBackgroundCheck].transform.position +
                    new Vector3(mapfixWidth, 0f, 0f);
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


    private bool CheckPoint()
    {
        bool check = false;

        foreach (GameObject obj in obstaclPrefabs)
        {
            if (obj.activeSelf && obj.transform.position.x + MapManager.Instance.totalWidth - mapfixWidth < gameObject.transform.position.x)
                obj.SetActive(false);
        }

        return true;
    }
}
