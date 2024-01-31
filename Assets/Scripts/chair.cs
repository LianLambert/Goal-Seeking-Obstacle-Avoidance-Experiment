using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class chair : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private GameObject goal;
    private float pushForce = 1.0f;
    private goal goalScript;
    private GameObject closestHuman;

    // Start is called before the first frame update
    void Start()
    {
        goal = GameObject.FindWithTag("goal");
        goalScript = goal.GetComponent<goal>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!goalScript.goalReached)
        {
            // calculate direction to closest human
            findClosestHuman();
            Vector3 humanPosNoY = new Vector3(closestHuman.transform.position.x, 2, closestHuman.transform.position.z);
            Vector3 directionToHuman = (humanPosNoY - transform.position).normalized;

            // check for obstacles and try to avoid by pushing in the other direction
            Collider[] hits = Physics.OverlapSphere(transform.position, 0.5f);
            Vector3 totalPushDirection = Vector3.zero;

            // for each obstacle (only chairs are considered obstacles for chairs)
            foreach (Collider collider in hits)
            {
                if (collider.gameObject.CompareTag("chair"))
                {
                    // find direction away from obstacle and add to total push direction
                    Vector3 pushDirection = (transform.position - collider.gameObject.transform.position);
                    totalPushDirection += pushDirection;
                }
            }

            // calculate translation based on direction to goal and push force
            Vector3 finalPushDirection = new Vector3(totalPushDirection.normalized.x, 0, totalPushDirection.normalized.z);
            MoveWithoutOverlap((directionToHuman + finalPushDirection * pushForce).normalized * speed * Time.deltaTime);
        
        }
    }

    void findClosestHuman()
    {
        // look through all the humans
        GameObject[] humans = GameObject.FindGameObjectsWithTag("human");
        closestHuman = humans[0];

        // record the one closest to the chair
        foreach (GameObject human in humans) {
            if (Vector3.Distance(human.transform.position, transform.position) < (Vector3.Distance(closestHuman.transform.position, transform.position))) {
                closestHuman = human;
            }
        }
    }

    void MoveWithoutOverlap(Vector3 movement)
    {
        // check what overlaps with the new position
        Vector3 newPosition = transform.position + movement;
        Collider[] collisions = Physics.OverlapSphere(newPosition, 0.5f);

        bool hasOverlap = false;

        // check if there's an overlap
        foreach (Collider collider in collisions)
        {
            if (collider.gameObject != this.gameObject)
            {
                hasOverlap = true;
                break;
            }
        }

        // If there's no overlap, move the chair
        if (!hasOverlap)
        {
            transform.Translate(movement);
        }
    }
}
