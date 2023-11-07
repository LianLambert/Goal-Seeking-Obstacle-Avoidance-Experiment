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
        if (!goalScript.goalReached) {

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
            transform.Translate((directionToGoal * speed + finalPushDirection * pushForce) * Time.deltaTime);
        }
    }
}
