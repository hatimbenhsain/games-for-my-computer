using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TiltScript : MonoBehaviour
{
    public float sensitivity = 0.5f; // Adjust this value to change rotation sensitivity
    public float lockedYAxisRotation = 0f; // Set this to the desired Y-axis rotation value


    private void Start()
    {
        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get mouse input and apply sensitivity
        float mouseXValue = Input.GetAxis("Mouse X") * sensitivity;
        float mouseYValue = Input.GetAxis("Mouse Y") * sensitivity;
        
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lockedYAxisRotation, transform.rotation.eulerAngles.z);


        // Ensure sensitivity affects rotation in all directions by directly applying these values
        // Rotate the object based on mouse movement, considering both axes
        transform.Rotate(-mouseYValue, 0, mouseXValue, Space.World);
    }
}