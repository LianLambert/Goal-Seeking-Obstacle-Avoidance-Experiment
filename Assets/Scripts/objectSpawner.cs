using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject chair;
    [SerializeField] private GameObject human;
    [SerializeField] private GameObject goal;
    [SerializeField] public int numChairs;
    [SerializeField] public int numHumans;
    private int minX = -24;
    private int maxX = 23;
    private int minZ = -49;
    private int maxZ = 48;
    private List<Vector3> usedPositions = new List<Vector3>();


    // Start is called before the first frame update
    void Start()
    {
        spawnPrefab(chair, numChairs);
        spawnPrefab(human, numHumans);
        spawnPrefab(goal, 1);
    }

    // Update is called once per frame
    void spawnPrefab(GameObject prefab, int numPrefabs)
    {
        for (int i = 0; i<numPrefabs; i++)
        {
            Vector3 spawnPos = GetRandomIntegerPosition();

            while (usedPositions.Contains(spawnPos))
            {
                spawnPos = GetRandomIntegerPosition();
            }

            usedPositions.Add(spawnPos);

            if (prefab == human)
            {
                spawnPos = spawnPos + new Vector3(0, 1, 0);
            }

            Instantiate(prefab, spawnPos, Quaternion.identity);
        }   
    }

    private Vector3 GetRandomIntegerPosition()
    {
        int randomX = Random.Range(minX, maxX + 1);
        int randomZ = Random.Range(minZ, maxZ + 1);

        return new Vector3(randomX, 1, randomZ);
    }
}
