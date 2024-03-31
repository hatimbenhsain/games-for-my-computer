using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltScript : MonoBehaviour
{
    public float sensitivity = 0.5f;

    public float mouseXValue;
    public float mouseYValue;
    
    // Update is called once per frame
    void Update()
    { 
        //may need to mess around with sensitivity levels so the ball doesn't clip through the platform
        mouseXValue = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        mouseYValue = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
       
       //using rotate is more consistent than physics, the physics kept breaking? 
       transform.Rotate(-mouseYValue, 0, mouseXValue);
       transform.rotation.Normalize();
       
    }
}
