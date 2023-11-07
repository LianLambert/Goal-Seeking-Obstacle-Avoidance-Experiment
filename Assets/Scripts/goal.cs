using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goal : MonoBehaviour
{
    public bool goalReached = false;

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "human")
        {
            goalReached = true;
        }
    }
}
