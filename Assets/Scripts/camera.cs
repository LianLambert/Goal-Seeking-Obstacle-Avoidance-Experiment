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
        // Handle camera movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(moveDirection);

        // Handle camera rotation with the mouse
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotation.x -= mouseY * sensitivity;
        rotation.y += mouseX * sensitivity;

        // Limit the vertical rotation to prevent camera flipping
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
        Vector3 finalRotation = new Vector3(rotation.x + 30f, rotation.y + 45f, rotation.z);

        // Apply rotation to the camera
        transform.eulerAngles = rotation;
        transform.eulerAngles += new Vector3(30, 45, 0);
    }
}
