using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Currently not using this script, it was meant for making sprites always face the camera
public class PaperScript : MonoBehaviour
{
    private Vector3 ogRotation;
    private Transform bodyTransform;
    // Start is called before the first frame update
    void Start()
    {
        ogRotation=transform.rotation.eulerAngles;
        bodyTransform=transform.parent.GetComponentInChildren<Rigidbody>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        float targetRotationX = ogRotation.x;
        float targetRotationY = bodyTransform.rotation.eulerAngles.y+ogRotation.y;
        float targetRotationZ = ogRotation.z;
        Vector3 rotation = new Vector3(targetRotationX, targetRotationY, targetRotationZ);
        transform.rotation = Quaternion.Euler(rotation);
        transform.localPosition=bodyTransform.localPosition;
    }
}
