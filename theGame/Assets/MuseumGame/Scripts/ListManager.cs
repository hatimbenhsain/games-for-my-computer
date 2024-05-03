using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ListManager : MonoBehaviour
{
    public GameObject AlienList;
    public GameObject CheckOffIntro;
    public GameObject CheckOffChair;
    public GameObject CheckoffScream;
    public GameObject CheckoffMobile;
    public GameObject CheckoffSink;
    public GameObject CheckoffUFO;

    // Track if alien list is active
    private bool isActive = false;

    // Track completion status of tasks
    private bool introCompleted = false;
    private bool chairCompleted = false;
    private bool screamCompleted = false;
    private bool mobileCompleted = false;
    private bool sinkCompleted = false;
    private bool UFOCompleted = false;

    void Start()
    {
        // Alien list is initially inactive
        AlienList.SetActive(false);
        // All check off items are initially inactive
        SetCheckOffItemsActive(false);
    }

    void Update()
    {
        // Check if the tab key is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Change the isActive flag
            isActive = !isActive;

            // Activate or deactivate the UI element based on the isActive flag
            AlienList.SetActive(isActive);

            // If IntroComplete is called and isActive is true, activate CheckOffIntro
            if (introCompleted && isActive)
            {
                CheckOffIntro.SetActive(true);
            }
            else
            {
                // Deactivate CheckOffIntro if AlienList is deactivated
                CheckOffIntro.SetActive(false);
            }
            
            if (chairCompleted && isActive)
            {
                CheckOffChair.SetActive(true);
            }
            else
            {
                // Deactivate CheckOffChair if AlienList is deactivated
                CheckOffChair.SetActive(false);
            }
            
            if (screamCompleted && isActive)
            {
                CheckoffScream.SetActive(true);
            }
            else
            {
                // Deactivate CheckOffChair if AlienList is deactivated
                CheckoffScream.SetActive(false);
            }
            
            if (mobileCompleted && isActive)
            {
                CheckoffMobile.SetActive(true);
            }
            else
            {
                // Deactivate CheckOffChair if AlienList is deactivated
                CheckoffMobile.SetActive(false);
            }
            
            if (sinkCompleted && isActive)
            {
                CheckoffSink.SetActive(true);
            }
            else
            {
                // Deactivate CheckOffChair if AlienList is deactivated
                CheckoffSink.SetActive(false);
            }
            
            if (UFOCompleted && isActive)
            {
                CheckoffUFO.SetActive(true);
            }
            else
            {
                // Deactivate CheckOffChair if AlienList is deactivated
                CheckoffUFO.SetActive(false);
            }
        }
    }


    [YarnCommand]
    public void IntroComplete()
    {
        // Set introCompleted to true
        introCompleted = true;
    }


    [YarnCommand]
    public void ChairComplete()
    {
        chairCompleted = true;
    }

    [YarnCommand]
    public void ScreamComplete()
    {
        screamCompleted = true;
    }

    [YarnCommand]
    public void MobileComplete()
    {
        mobileCompleted = true;
    }

    [YarnCommand]
    public void SinkComplete()
    {
        sinkCompleted = true;
    }

    [YarnCommand]
    public void UFOComplete()
    {
        UFOCompleted = true;
    }

    // Helper method to set all check off items active/inactive
    private void SetCheckOffItemsActive(bool active)
    {
        CheckOffIntro.SetActive(active);
        CheckOffChair.SetActive(active);
        CheckoffScream.SetActive(active);
        CheckoffMobile.SetActive(active);
        CheckoffSink.SetActive(active);
        CheckoffUFO.SetActive(active);
    }
}
