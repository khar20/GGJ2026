using UnityEngine;
using System.Collections.Generic;

public class SegmentSpawner : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public GameObject obstaclePrefab; // Drag Obstacle_Basic here
    [Range(0, 100)] 
    public int spawnChance = 60; // Probability that a point gets an obstacle

    [Header("Spawn Locations")]
    public List<Transform> spawnPoints; // Drag the 6 points here

    void Start()
    {
        SpawnObstacles();
    }

    void SpawnObstacles()
    {
        foreach (Transform point in spawnPoints)
        {
            // Roll a "dice" to see if we spawn an obstacle here
            if (Random.Range(0, 100) < spawnChance)
            {
                // Instantiate the obstacle as a child of this segment
                GameObject obstacle = Instantiate(obstaclePrefab, point.position, point.rotation);
                obstacle.transform.SetParent(transform);
            }
        }
    }
}