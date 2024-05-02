using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class AgentLetterViewer : MonoBehaviour
{
    public GameObject agentLetter;
    
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
}
