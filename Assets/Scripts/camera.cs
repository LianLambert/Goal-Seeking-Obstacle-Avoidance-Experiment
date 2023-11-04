using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 2f;
    public float maxTiltAngle = 45f;

    private Transform cameraTransform;
    private Vector3 cameraOffset;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        cameraOffset = cameraTransform.position - transform.position;
    }

    private void Update()
    {
        // Camera movement based on WASD keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        Vector3 moveVector = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * moveDirection;
        transform.position += moveVector * moveSpeed * Time.deltaTime;

        // Calculate the camera look rotation based on the target object
        Vector3 targetPosition = transform.position + Vector3.up * cameraOffset.y;
        Vector3 lookDirection = targetPosition - cameraTransform.position;

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            // Apply a limited tilt angle to the camera
            float currentTilt = Vector3.SignedAngle(Vector3.up, cameraOffset.normalized, Vector3.forward);
            float tiltChange = -Input.GetAxis("Mouse Y") * rotationSpeed;

            if (Mathf.Abs(currentTilt + tiltChange) <= maxTiltAngle)
            {
                cameraOffset = Quaternion.AngleAxis(tiltChange, Vector3.right) * cameraOffset;
            }

            // Update the camera's position and rotation
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * rotationSpeed, 0);
            cameraTransform.position = transform.position + cameraOffset;
            cameraTransform.rotation = targetRotation;
        }
    }
}
