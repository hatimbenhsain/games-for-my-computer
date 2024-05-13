using UnityEngine;

public class ObjectResetter : MonoBehaviour
{
    public GameObject assignableObject; // Drag the object to monitor here
    public Transform targetLocation;    // Drag the empty GameObject here to define the target location

    // This function is called when another object exits the trigger collider
    private void OnTriggerExit(Collider other)
    {
        // Check if the object leaving the collider is the assignable object
        if (other.gameObject == assignableObject)
        {
            // Move the assignable object to the target location
            assignableObject.transform.position = targetLocation.position;
        }
    }
}