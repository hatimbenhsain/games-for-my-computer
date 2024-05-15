using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class UITurnOff : MonoBehaviour
{
    public GameObject UICanvas;
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [YarnCommand]
    public void TurnOffUI()
    {
        if (UICanvas != null)
        {
            // Activate the boxScanner object
            UICanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("UICanvas object reference is not set!");
        }
    }
}
