using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class HeadCameraFollow : MonoBehaviour
{

    private Vector3 cameraDir;
    private Transform followPlayer; // The target position and rotation for the player
    public Vector3 cameraPosition;
    public Vector3 cameraPositionFlipped;
    public Quaternion cameraRotation;
    private ThirdPersonController thirdPersonController;
    [SerializeField] GameObject PlayerController;
    // Start is called before the first frame update
    void Start()
    {
        thirdPersonController = PlayerController.GetComponent<ThirdPersonController>(); // Get the third person controller script
        GameObject playerCameraRootObject = GameObject.Find("PlayerController"); // Get the player transform
        if (playerCameraRootObject != null)
        {
            followPlayer = playerCameraRootObject.transform;
        }
        else
        {
            Debug.LogError("PlayerController not found in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        cameraDir = Camera.main.transform.forward;
        cameraDir.y = 0;

        transform.rotation = Quaternion.LookRotation(cameraDir); // set rotation based on camera
        if (thirdPersonController.flipped)
        {
            transform.position = followPlayer.position + cameraPosition; // set transform to the player transform
        }
        else
        {
            transform.position = followPlayer.position + cameraPositionFlipped;
        }

    }
}
