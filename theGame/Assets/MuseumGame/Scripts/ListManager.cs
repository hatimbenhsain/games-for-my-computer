using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListManager : MonoBehaviour
{
   
    public GameObject AlienList;
    

    // track if alien list is active
    private bool isActive = false;

    void Start()
    {
        // Alien list is initally inactive
        AlienList.SetActive(false);
    }

    void Update()
    {
        // Check if the tab key is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // change the the isActive flag
            isActive = !isActive;

            // Activate or deactivate the UI element based on the isActive flag
            AlienList.SetActive(isActive);
        }
    }
}
