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
    private float goalTimer = 0;
    private bool goalRespawning = false;
    public List<GameObject> activeObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        goal = GameObject.FindWithTag("goal");
        respawnGoal();
        spawnPrefab(human, numHumans);
        spawnPrefab(chair, numChairs);
    }

    // Update is called once per frame
    void Update()
    {
        // if a human reaches the goal or the goal was not reached in 10s, respawn it
        goalTimer += Time.deltaTime;
        if ((goal.GetComponent<goal>().goalReached || goalTimer >= 10.0f) && goalRespawning == false)
        {
            goalRespawning = true;
            Invoke("respawnGoal", 1);
        }

    }


    void spawnPrefab(GameObject prefab, int numPrefabs)
    {
        Vector3 spawnPos;

        // make numPrefab number of prefabs
        for (int i = 0; i<numPrefabs; i++)
        {
            // get unoccupied spawn position
            Vector2 spawnSquare = GetSpawnSquare();

            // humans need to spawn a bit higher becuase they are taller
            if (prefab == human)
            {
                spawnPos = new Vector3(spawnSquare.x, 2, spawnSquare.y);
            }
            else
            {
                spawnPos = new Vector3(spawnSquare.x, 1, spawnSquare.y);
            }

            // intantiate the prefab
            activeObjects.Add(Instantiate(prefab, spawnPos, Quaternion.identity));
        }   
    }

    private Vector2 GetRandomIntegerPosition()
    {
        // ensure spawn position is inside bounds
        int randomX = Random.Range(minX, maxX + 1);
        int randomZ = Random.Range(minZ, maxZ + 1);

        return new Vector2(randomX, randomZ);
    }

    private Vector2 GetSpawnSquare()
    {
        Vector2 spawnPos = GetRandomIntegerPosition();

        // Ensure that the while loop has a reasonable upper limit to prevent infinite loops
        int maxAttempts = 100;
        int attempts = 0;

        while (Physics.OverlapSphere(new Vector3(spawnPos.x, 0, spawnPos.y), 1).Length != 0 && attempts < maxAttempts)
        {
            spawnPos = GetRandomIntegerPosition();
            attempts++;
        }

        if (attempts >= maxAttempts)
        {
            // Handle the case where no empty spawn position was found after a certain number of attempts
            Debug.LogError("Failed to find an empty spawn position after " + maxAttempts + " attempts.");
            return Vector2.zero; // Or some other default value
        }

        return spawnPos;
    }

    private void respawnGoal()
    {
        Time.timeScale = 0;
        // spawn goal in new spot, updating occupiedPositions list
        Vector3 spawnPos = GetSpawnSquare();
        goal.transform.position = new Vector3(spawnPos.x, 1, spawnPos.y);

        // reset goal timer, goalReached
        goalTimer = 0.0f;
        goal.GetComponent<goal>().goalReached = false;
        goalRespawning = false;
        Time.timeScale = 1;
    }

   
}
