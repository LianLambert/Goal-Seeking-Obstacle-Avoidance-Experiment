using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class human : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject goal;
    private goal goalScript;
    private float pushForce = 1.0f;

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

            // calculate direction to goal
            Vector3 goalPosNoY = new Vector3(goal.transform.position.x, 2, goal.transform.position.z);
            Vector3 directionToGoal = (goalPosNoY - transform.position).normalized;

            // check for obstacles and try to avoid by pushing in the other direction
            Collider[] hits = Physics.OverlapSphere(transform.position, 2);
            Vector3 totalPushDirection = Vector3.zero;

            // for each obstacle
            foreach (Collider collider in hits)
            {
                // find direction away from obstacle and add to total push direction
                Vector3 pushDirection = (transform.position - collider.gameObject.transform.position);
                totalPushDirection += pushDirection;
            }

            // calculate translation based on direction to goal and push force
            Vector3 finalPushDirection = new Vector3(totalPushDirection.normalized.x, 0, totalPushDirection.normalized.z);
            MoveWithoutOverlap((directionToGoal + finalPushDirection * pushForce).normalized * speed * Time.deltaTime);
        }
    }

    void MoveWithoutOverlap(Vector3 movement)
    {
        Vector3 originalPosition = transform.position;

        for (int angle = 0; angle < 360; angle += 15)
        {
            // rotate the movement vector by angle degrees
            Vector3 rotatedMovement = Quaternion.Euler(0, angle, 0) * movement;
            Vector3 newPosition = originalPosition + rotatedMovement;

            // check for overlap with the new position
            Collider[] rotatedHits = Physics.OverlapSphere(newPosition, 0.4f);
            bool isOverlap = false;

            // check if there's an overlap with chairs (not goal)
            foreach (Collider collider in rotatedHits)
            {
                if (collider.gameObject.CompareTag("chair") && collider.gameObject != this.gameObject)
                {
                    isOverlap = true;
                    break;
                }
            }

            // if a vector is found that does not lead to overlap, move there
            if (!isOverlap)
            {
                transform.Translate(rotatedMovement);
                return;
            }
        }
    }
}