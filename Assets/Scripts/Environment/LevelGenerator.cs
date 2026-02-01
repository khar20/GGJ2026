using System.Collections.Generic;
using UnityEngine;

public partial class LevelGenerator : MonoBehaviour
{
    public GameObject[] segmentPrefabs; // Array of floor pieces
    public Transform player;            // Reference to player

    private List<GameObject> activeSegments = new List<GameObject>();
    private float spawnZ = 0;           // Where the next segment starts
    private readonly float segmentLength = 30;   // Length of your floor prefab
    private readonly int segmentsOnScreen = 5;   // How many segments exist at once

    void Start()
    {
        // Spawn initial segments
        for (int i = 0; i < segmentsOnScreen; i++)
        {
            SpawnSegment(Random.Range(0, segmentPrefabs.Length));
        }
    }

    void Update()
    {
        // If player has moved far enough, spawn new and delete old
        if (player.position.z - 35 > (spawnZ - (segmentsOnScreen * segmentLength)))
        {
            SpawnSegment(Random.Range(0, segmentPrefabs.Length));
            DeleteOldSegment();
        }
    }

    void SpawnSegment(int prefabIndex)
    {
        GameObject go = Instantiate(segmentPrefabs[prefabIndex], transform.forward * spawnZ, transform.rotation);
        activeSegments.Add(go);
        spawnZ += segmentLength;
    }

    void DeleteOldSegment()
    {
        Destroy(activeSegments[0]);
        activeSegments.RemoveAt(0);
    }
}