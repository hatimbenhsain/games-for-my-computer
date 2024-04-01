using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganGiftManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> peopleList = new List<GameObject>();
    
    // Use a serialized list for the organ array to manage it in the Inspector
    [SerializeField]
    private List<GameObject> organArray = new List<GameObject>();

    private int currentOrganIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is in the peopleList
        if (peopleList.Contains(other.gameObject))
        {
            Debug.Log("Organ collided with a person!");

            // Check if there are organs left in the array
            if (currentOrganIndex < organArray.Count)
            {
                // Freeze the position of the collided organ
                FreezeOrganPosition(organArray[currentOrganIndex]);

                // Remove the OrganMover script from the collided organ
                RemoveOrganMoverScript(organArray[currentOrganIndex]);

                // Activate the next organ in the array
                ActivateNextOrgan();
            }
            else
            {
                Debug.Log("All organs processed!");
                // Here you can add code for what happens when all organs are processed.
            }
        }
    }

    // Freeze the position of the specified organ
    private void FreezeOrganPosition(GameObject organ)
    {
        Rigidbody organRigidbody = organ.GetComponent<Rigidbody>();
        if (organRigidbody != null)
        {
            organRigidbody.isKinematic = true;
        }
    }

    // Remove the OrganMover script from the specified organ
    private void RemoveOrganMoverScript(GameObject organ)
    {
        OrganMover organMover = organ.GetComponent<OrganMover>();
        if (organMover != null)
        {
            Destroy(organMover);
            Debug.Log("OrganMover script removed from " + organ.name);
        }
        else
        {
            Debug.LogWarning("OrganMover script not found on " + organ.name);
        }
    }

    // Activate the next organ in the array
    private void ActivateNextOrgan()
    {
        // Increment the index to move to the next organ in the array
        currentOrganIndex++;

        // Check if there are more organs in the array
        if (currentOrganIndex < organArray.Count)
        {
            // Activate the next organ
            organArray[currentOrganIndex].SetActive(true);
            Debug.Log("Next organ activated: " + organArray[currentOrganIndex].name);
        }
        else
        {
            Debug.Log("All organs processed!");
            // Here you can add code for what happens when all organs are processed.
        }
    }
}
