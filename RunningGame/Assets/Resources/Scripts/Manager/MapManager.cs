using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private string stage = "Prefab\\Obstacle\\Stage01";
    
    [SerializeField] GameObject[] obstaclePrefabs;
    [SerializeField] GameObject[] backGrounds;

    private List<GameObject> obstacles = new List<GameObject>();

    MapManager grid;

    Transform nextObstacleLocation;

    private void Awake()
    {
        grid = FindObjectOfType<MapManager>();
        
        obstaclePrefabs = Resources.LoadAll<GameObject>(stage);

        foreach (GameObject prefab in obstaclePrefabs)
        {
            GameObject instance = Instantiate(prefab, grid.transform);
            instance.name = prefab.name;
            instance.SetActive(instance.name == "Obstacle_00" ? true:false); 
            obstacles.Add(instance);
        }

        int firstRandomMab = Random.Range(1, obstacles.Count);
        obstacles[firstRandomMab].SetActive(true);
        obstacles[firstRandomMab].transform.position += new Vector3(17.92f, 0, 0);
    }



    public void SetBackGround(GameObject background)
    {
        
    }
}
