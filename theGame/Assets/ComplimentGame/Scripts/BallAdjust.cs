using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAdjust : MonoBehaviour
{
    public Transform board; 
    public float movementScale = 1.0f; //movement strength

    private float lastTiltX = 0; 
    private float lastTiltZ = 0; 

    void Start()
    {
        // initialize the last tilt values
        lastTiltX = board.eulerAngles.x;
        lastTiltZ = board.eulerAngles.z;
    }

    void Update()
    {
        MoveBallBasedOnBoardTiltChange();
    }

    private void MoveBallBasedOnBoardTiltChange()
    {
        float currentTiltX = board.eulerAngles.x;
        float currentTiltZ = board.eulerAngles.z; 

        float deltaTiltX = currentTiltX - lastTiltX;
        float deltaTiltZ = currentTiltZ - lastTiltZ;

        // ball relative position
        Vector3 relativePosition = transform.position - board.position;

        if (deltaTiltX != 0 || deltaTiltZ != 0)
        {
            // move the ball up and down based on tilt angle
            float radiansX = deltaTiltX * Mathf.Deg2Rad;
            float radiansZ = deltaTiltZ * Mathf.Deg2Rad;

            float movementYX = -Mathf.Sin(radiansX) * Mathf.Abs(relativePosition.x) * 0.1f * movementScale;
            float movementYZ = -Mathf.Sin(radiansZ) * Mathf.Abs(relativePosition.z) * movementScale;

            Vector3 movement = new Vector3(0, movementYX + movementYZ, 0);

            transform.position += movement * Time.deltaTime;
        }

        // update last tilt
        lastTiltX = currentTiltX;
        lastTiltZ = currentTiltZ;
    }


}
