using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class camera : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of camera movement
    public float sensitivity = 2f; // Mouse rotation sensitivity
    private Vector3 rotation = Vector3.zero;

    void Update()
    {
        // make arrow keys move camera position
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(moveDirection);

        // make mouse handle camera rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotation.x -= mouseY * sensitivity;
        rotation.y += mouseX * sensitivity;

        // limit rotation and ensure it's based on isometric view
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
        Vector3 finalRotation = new Vector3(rotation.x + 30f, rotation.y + 45f, rotation.z);

        transform.eulerAngles = rotation;
        transform.eulerAngles += new Vector3(30, 45, 0);
    }
}
