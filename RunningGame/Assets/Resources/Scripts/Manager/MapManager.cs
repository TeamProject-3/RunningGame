using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private string stage = "Prefab\\Obstacle\\Stage01";
    
    protected GameObject[] obstaclePrefabs;

    private List<GameObject> obstacles = new List<GameObject>();

    MapManager grid;

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
    }

    public List<GameObject> GetObstaclePrefabs()
    {
        return obstacles;
    }
}
