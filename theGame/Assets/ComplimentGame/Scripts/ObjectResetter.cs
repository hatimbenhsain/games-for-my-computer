using UnityEngine;

public class ObjectResetter : MonoBehaviour
{
    public GameObject assignableObject; // Drag the object to monitor here
    public Transform targetLocation;    // Drag the empty GameObject here to define the target location
    private Transform startingPos;

    // This function is called when another object exits the trigger collider
    private void Start()
    {
        startingPos.position = assignableObject.transform.position;
    }
    private void Update()
    {
        if (assignableObject.transform.position.y < (startingPos.position.y - 20f) || (startingPos.position.y+100f) < assignableObject.transform.position.y)
        {
            assignableObject.transform.position = targetLocation.position;
        }
    }
}