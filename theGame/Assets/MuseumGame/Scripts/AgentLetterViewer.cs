using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class AgentLetterViewer : MonoBehaviour
{
    public GameObject agentLetter;
    public GameObject faxMessage;
    public GameObject ScannerUI;
    public GameObject boxScanner;
    public GameObject boxLetter;

    public bool isPickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [YarnCommand]
    public void ShowLetter()
    {
        // Check if the AgentLetter object exists
        if (agentLetter != null)
        {
            // Activate the AgentLetter object
            agentLetter.SetActive(true);
        }
        else
        {
            Debug.LogError("AgentLetter object reference is not set!");
        }
    }

    [YarnCommand]
    public void HideLetter()
    {
        // Check if the AgentLetter object exists
        if (agentLetter != null)
        {
            // Deactivate the AgentLetter object
            agentLetter.SetActive(false);
        }
        else
        {
            Debug.LogError("AgentLetter object reference is not set!");
        }
    }
    
    [YarnCommand]
        public void ShowScanner()
        {
            // Check if the AgentLetter object exists
            if (ScannerUI != null)
            {
                // Activate the AgentLetter object
                ScannerUI.SetActive(true);
            }
            else
            {
                Debug.LogError("ScannerUI object reference is not set!");
            }
        }
        
    [YarnCommand]
        public void HideScanner()
        {
            // Check if the AgentLetter object exists
            if (ScannerUI != null)
            {
                // Activate the AgentLetter object
                ScannerUI.SetActive(false);
            }
            else
            {
                Debug.LogError("ScannerUI object reference is not set!");
            }
        }

           [YarnCommand]
    public void ShowFax()
    {
        // Check if the AgentLetter object exists
        if (faxMessage != null)
        {
            // Activate the AgentLetter object
            faxMessage.SetActive(true);
        }
        else
        {
            Debug.LogError("faxMessage object reference is not set!");
        }
    }

    [YarnCommand]
    public void HideFax()
    {
        // Check if the AgentLetter object exists
        if (faxMessage != null)
        {
            // Deactivate the AgentLetter object
            faxMessage.SetActive(false);
        }
        else
        {
            Debug.LogError("faxMessage object reference is not set!");
        }
    }

    [YarnCommand]
    public void HideBoxLetter()
    {
        if (boxLetter != null)
        {
            boxLetter.SetActive(false);
        }
        else
        {
            Debug.Log("boxLetter object reference is not set!");
        }
    }
    
    [YarnCommand]
    public void HideBoxScanner()
    {
        if (boxScanner != null)
        {
            boxScanner.SetActive(false);

            isPickedUp = true;

        }
        else
        {
            Debug.LogError("boxScanner object reference is not set!");
        }
    }

    [YarnCommand]
    public void DropScannerAtLocation()
    {
        if (boxScanner != null)
        {
            // Activate the boxScanner object
            boxScanner.SetActive(true);

            // Set the position of the scanner to the desired location
            Vector3 desiredPosition = new Vector3(-6.05070019f, -0.0320000015f, -23.6200008f);
            boxScanner.transform.position = desiredPosition;

            Debug.Log("Scanner position set to: " + desiredPosition);
        }
        else
        {
            Debug.LogError("boxScanner object reference is not set!");
        }
    }
    
}
