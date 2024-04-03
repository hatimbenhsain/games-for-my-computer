using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TiltScript : MonoBehaviour
{
    public float sensitivity = 0.5f;

    public float mouseXValue;
    public float mouseYValue;

    private Vector3 lastMousePosition;

    private void Start()
    {
        //keep confined in game view
        //Cursor.lockState = CursorLockMode.Confined;

        //keep it at the center
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    { 
        //may need to mess around with sensitivity levels so the ball doesn't clip through the platform
        mouseXValue = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        mouseYValue = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        mouseXValue = Mathf.Min(mouseXValue, 0.1f);
        mouseYValue = Mathf.Min(mouseYValue, 0.1f);
        
        // Sets target angle according to inputs + camera angle
       transform.Rotate(-mouseYValue, 0, mouseXValue);
       // Smooths the target angle
       transform.rotation.Normalize();

       lastMousePosition = Input.mousePosition;


    }
}
