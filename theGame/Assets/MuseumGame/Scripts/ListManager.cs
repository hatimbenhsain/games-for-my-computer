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

    // Track completion status of tasks
    private bool introCompleted = false;
    private bool chairCompleted = false;
    private bool screamCompleted = false;
    private bool mobileCompleted = false;
    private bool sinkCompleted = false;
    private bool UFOCompleted = false;

    void Start()
    {
        // All check off items are initially inactive
        SetCheckOffItemsActive(false);
    }

    void Update()
    {
        // Check if the tab key is being held down
        if (Input.GetKey(KeyCode.Tab))
        {
            // Activate the UI element and check off items
            AlienList.SetActive(true);
            UpdateCheckOffItems();
        }
        else
        {
            // Deactivate the UI element and check off items
            AlienList.SetActive(false);
            SetCheckOffItemsActive(false);
        }
    }

    // Method to update the visibility of check off items
    private void UpdateCheckOffItems()
    {
        // If the AlienList is active, update the visibility of check off items based on completion status
        if (AlienList.activeSelf)
        {
            CheckOffIntro.SetActive(introCompleted);
            CheckOffChair.SetActive(chairCompleted);
            CheckoffScream.SetActive(screamCompleted);
            CheckoffMobile.SetActive(mobileCompleted);
            CheckoffSink.SetActive(sinkCompleted);
            CheckoffUFO.SetActive(UFOCompleted);
        }
    }

    [YarnCommand]
    public void IntroComplete()
    {
        // Set introCompleted to true
        introCompleted = true;
        UpdateCheckOffItems();
    }

    [YarnCommand]
    public void ChairComplete()
    {
        chairCompleted = true;
        UpdateCheckOffItems();
    }

    [YarnCommand]
    public void ScreamComplete()
    {
        screamCompleted = true;
        UpdateCheckOffItems();
    }

    [YarnCommand]
    public void MobileComplete()
    {
        mobileCompleted = true;
        UpdateCheckOffItems();
    }

    [YarnCommand]
    public void SinkComplete()
    {
        sinkCompleted = true;
        UpdateCheckOffItems();
    }

    [YarnCommand]
    public void UFOComplete()
    {
        UFOCompleted = true;
        UpdateCheckOffItems();
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
