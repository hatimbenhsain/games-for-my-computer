using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameObject playerController; // Reference to your FirstPersonController

    private void Start()
    {
        // Initially, set the UI element to be visible and disable player movement.
        gameObject.SetActive(true);
        playerController.GetComponent<FirstPersonController>().playerCanMove = false;
        playerController.GetComponent<FirstPersonController>().cameraCanMove = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // When the "E" key is pressed, hide the UI element and enable player movement.
            gameObject.SetActive(false);
            playerController.GetComponent<FirstPersonController>().playerCanMove = true;
            playerController.GetComponent<FirstPersonController>().cameraCanMove = true;
        }
    }
}