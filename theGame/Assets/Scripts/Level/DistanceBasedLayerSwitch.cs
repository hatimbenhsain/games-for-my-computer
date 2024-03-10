using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceBasedLayerSwitch : MonoBehaviour
{
    public Transform playerTransform; // Assign this in the inspector with your player's transform
    public float outlinedDistance = 6f; // Distance at which the object becomes outlined
    public float interactableDistance = 3f; // Distance at which the object becomes interactable

    private int defaultLayer;
    private int outlinedLayer;
    private int interactableLayer;

    void Start()
    {
        // Cache the layer indices
        defaultLayer = LayerMask.NameToLayer("Default");
        outlinedLayer = LayerMask.NameToLayer("Highlighted");
        interactableLayer = LayerMask.NameToLayer("Interactable");
        playerTransform = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    void Update()
    {
        // Calculate the distance to the player
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // Check distance and assign layer accordingly
        if (distance <= interactableDistance)
        {
            gameObject.layer = interactableLayer;
        }
        else if (distance <= outlinedDistance && distance > interactableDistance)
        {
            gameObject.layer = outlinedLayer;
        }
        else
        {
            gameObject.layer = defaultLayer;
        }
    }
}
