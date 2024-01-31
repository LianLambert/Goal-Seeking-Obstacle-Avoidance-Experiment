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
    private int minX = -14;
    private int maxX = 13;
    private int minZ = -24;
    private int maxZ = 23;
    private float goalTimer = 0;
    private bool goalRespawning = false;
    public List<GameObject> activeObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        goal = GameObject.FindWithTag("goal");

        // spawn game components
        respawnGoal();
        spawnPrefab(human, numHumans);
        spawnPrefab(chair, numChairs);
    }

    public int avgFrameRate;

    // Update is called once per frame
    void Update()
    {
        // if a human reaches the goal or the goal was not reached in 10s, respawn it
        goalTimer += Time.deltaTime;
        if ((goal.GetComponent<goal>().goalReached || goalTimer >= 10.0f) && goalRespawning == false)
        {
            DisableMovementScripts();
            goalRespawning = true;
            Invoke("respawnGoal", 1);
        }

        float current = 0;
        current = Time.frameCount / Time.time;
        avgFrameRate = (int)current;
        Debug.Log((int)(1f / Time.unscaledDeltaTime));
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

        // set an upper limit to ensure no infinite loops
        int maxAttempts = 500;
        int attempts = 0;

        // find a spawn position that is unoccupied 
        while (Physics.OverlapSphere(new Vector3(spawnPos.x, 0, spawnPos.y), 1).Length != 0 && attempts < maxAttempts)
        {
            spawnPos = GetRandomIntegerPosition();
            attempts++;
        }

        // if unable to find suitable spawn position, return vector 0 and note in console
        if (attempts >= maxAttempts)
        {
            Debug.Log("Failed to find an empty spawn position after " + maxAttempts + " attempts.");
            return Vector2.zero;
        }

        return spawnPos;
    }

    private void respawnGoal()
    {
        Debug.Log(System.DateTime.Now);
        // disable chair and human scripts
        DisableMovementScripts();

        // spawn goal in new spot
        Vector3 spawnPos = GetSpawnSquare();
        goal.transform.position = new Vector3(spawnPos.x, 1, spawnPos.y);

        // reset goal timer, goalReached and enable chair/human scripts
        goalTimer = 0.0f;
        goal.GetComponent<goal>().goalReached = false;
        goalRespawning = false;
        EnableMovementScripts();

    }

    private void DisableMovementScripts()
    {
        // disable chair and human scripts
        foreach (GameObject activeObject in activeObjects)
        {
            if (activeObject.CompareTag("human"))
            {
                activeObject.GetComponent<human>().enabled = false;
            }
            else if (activeObject.CompareTag("chair"))
            {
                activeObject.GetComponent<chair>().enabled = false;
            }
        }
    }

    private void EnableMovementScripts()
    {
        // enable chair and human scripts
        foreach (GameObject activeObject in activeObjects)
        {
            if (activeObject.CompareTag("human"))
            {
                activeObject.GetComponent<human>().enabled = true;
            }
            else if (activeObject.CompareTag("chair"))
            {
                activeObject.GetComponent<chair>().enabled = true;
            }
        }
    }


}
