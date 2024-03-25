using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntestineMover : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool ableToMove = true;

    [SerializeField]
    private List<GameObject> peopleList = new List<GameObject>();
    
    [SerializeField]
    private GameObject heart_0;

    // Update is called once per frame
    void Update()
    {
        if (ableToMove)
        {
            // Get input values for horizontal (A/D) and vertical (W/S) movement
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Calculate the movement direction based on the input values
            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

            // Move the player in the calculated direction
            transform.Translate(movement * moveSpeed * Time.deltaTime);
        }
        else
        {
            // If ableToMove is false, activate the liver_0 GameObject
            if (heart_0 != null)
            {
                heart_0.SetActive(true);
            }
        }
    }

    // Called when the Collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is in the peopleList
        if (peopleList.Contains(other.gameObject))
        {
            Debug.Log("Player collided with a person!");
            
            // Set ableToMove to false, preventing further movement
            ableToMove = false;
        }
    }
}