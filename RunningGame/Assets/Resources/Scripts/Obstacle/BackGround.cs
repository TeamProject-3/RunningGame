using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] private GameObject[] backGround;
    private List<GameObject> obstaclPrefabs;
   
    float fixWidth = 17.92f;
    bool first = true;

    private void Start()
    {
        MapManager mapManager = FindObjectOfType<MapManager>();
        obstaclPrefabs = mapManager.GetObstaclePrefabs();
        obstaclPrefabs.RemoveAll(mapManager => mapManager.name == "Obstacle_00");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BackGround")) 
        {
            GameObject check = collision.gameObject;

            int findBackground = System.Array.IndexOf(backGround, check);
            int nextBackgroundCheck = (findBackground + 1) % backGround.Length;
            
            ObstacleShuffle();
            
            for (int i = 0; i < backGround.Length; i++)
            {
                if (i == findBackground || i == nextBackgroundCheck) continue;

                backGround[i].transform.position =
                    backGround[nextBackgroundCheck].transform.position + 
                    new Vector3(fixWidth, 0f, 0f);
                
                foreach (GameObject obj in obstaclPrefabs)
                {
                    if (obj.activeSelf && obj.transform.position.x < gameObject.transform.position.x)
                    {
                        obj.SetActive(false);
                    }
                }

                if (first)
                {
                    first = false;
                    return;
                }

                //TEST(3, findBackground);

                
                foreach (GameObject obj in obstaclPrefabs)
                {

                    if (!obj.activeSelf)
                    {
                        obj.SetActive(true);
                        obj.transform.position =
                            backGround[findBackground].transform.position +
                             new Vector3(fixWidth, 0f, 0f);
                        break;
                    }
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

    private void TEST(int obstacleNum, int a)
    {
        obstaclPrefabs[obstacleNum].SetActive(true);
        obstaclPrefabs[obstacleNum].transform.position =
            backGround[a].transform.position +
             new Vector3(fixWidth, 0f, 0f);
    }
}
