using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private string stage_One = "Prefab\\Obstacle\\Stage01";
    private string stage_Two = "Prefab\\Obstacle\\Stage01";

    protected GameObject[] obstaclePrefabs;

    private List<GameObject> obstacles = new List<GameObject>();

    MapManager grid;

    private void Awake()
    {
        grid = FindObjectOfType<MapManager>();
        
        //스테이지 가져올때 변경필요함
        obstaclePrefabs = Resources.LoadAll<GameObject>(stage_One);

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
