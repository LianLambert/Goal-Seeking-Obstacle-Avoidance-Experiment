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
            Vector3 directionToHuman = (closestHuman.transform.position - transform.position).normalized;

            // check for obstacles and try to avoid by pushing in the other direction
            Collider[] hits = Physics.OverlapSphere(transform.position, 2);
            Vector3 totalPushDirection = Vector3.zero;

            // for each obstacle
            foreach (Collider collider in hits)
            {
                if (collider.gameObject.tag == "chair")
                {
                    // find direction away from obstacle and add to total push direction
                    Vector3 pushDirection = (transform.position - collider.gameObject.transform.position);
                    totalPushDirection += pushDirection;
                }
            }

            // calculate translation based on direction to goal and push force
            Vector3 finalPushDirection = new Vector3(totalPushDirection.normalized.x, 0, totalPushDirection.normalized.z);
            transform.Translate((directionToHuman * speed + finalPushDirection * pushForce) * Time.deltaTime);
        
        }
    }

    void findClosestHuman()
    {
        GameObject[] humans = GameObject.FindGameObjectsWithTag("human");
        closestHuman = humans[0];

        foreach (GameObject human in humans) {
            if (Vector3.Distance(human.transform.position, transform.position) < (Vector3.Distance(closestHuman.transform.position, transform.position))) {
                closestHuman = human;
            }
        }
    }
}
