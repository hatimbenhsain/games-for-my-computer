using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TiltScript : MonoBehaviour
{
    public float sensitivity = 0.5f; // Adjust this value to change rotation sensitivity
    public float lockedYAxisRotation = 0f; // Set this to the desired Y-axis rotation value
    private float countDown = 3.5f; // the time for tilt locking and count down
    
    // Public variables to define max rotation angles for X and Z directions
    public float maxRotationX = 45f; // Maximum allowed rotation in the X direction
    public float maxRotationZ = 45f; // Maximum allowed rotation in the Z direction

    // Keep track of the current rotation in X and Z to apply constraints
    private float currentRotationX = 0f;
    private float currentRotationZ = 0f;

    public float rotationSpeed = 1f; // Speed at which the rotation lerps to the target value
    private bool rotationLock = true; // lock the rotation
    private float startTime = 0f; // time when the game starts

    private void Start()
    {
        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // record the start time
        startTime = Time.time;
    }

    void Update()
    {
        // countdown
        if (rotationLock && Time.time - startTime > countDown)
        {
            rotationLock = false;
            Debug.Log("unlock rotation");
        }

        if (rotationLock)
        {
            Debug.Log(Time.time - startTime);
            // return update when rotationlock is true
            return;
        }

        // Get mouse input and apply sensitivity
        float mouseXValue = Input.GetAxis("Mouse X") * sensitivity;
        float mouseYValue = Input.GetAxis("Mouse Y") * sensitivity;

        // Update current rotation, applying sensitivity and clamping to specified limits
        currentRotationX = Mathf.Clamp(currentRotationX - mouseYValue, -maxRotationX, maxRotationX);
        currentRotationZ = Mathf.Clamp(currentRotationZ + mouseXValue, -maxRotationZ, maxRotationZ);

        // Calculate the target rotation based on currentRotationX, Y, and Z
        Quaternion targetRotation = Quaternion.Euler(currentRotationX, lockedYAxisRotation, currentRotationZ);

        // Smoothly interpolate towards the target rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}