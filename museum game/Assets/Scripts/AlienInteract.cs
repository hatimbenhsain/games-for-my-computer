using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienInteract : MonoBehaviour
{
    public float interactionDistance = 3f; // Adjust the interaction distance as needed.
    public LayerMask interactableLayer;    // Define a layer for objects that can be interacted with.

    private Camera playerCamera;

    private void Start()
    {
        // Get a reference to the first-person camera.
        playerCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        // Check for player input to interact.
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Perform a raycast from the camera's position and direction.
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayer))
            {
                // Check if the hit object has a script or component that allows interaction.
                Alien alien = hit.collider.GetComponent<Alien>();

                if (alien != null)
                {
                    // Call the interaction function on the object.
                    alien.Interact();
                }
            }
        }
    }
}
