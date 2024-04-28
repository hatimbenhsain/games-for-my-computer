using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI : MonoBehaviour
{
    public GameObject UI0A;
    public GameObject UI0S;
    public GameObject UI0D;
    public GameObject UI0W;
    public GameObject UI0Q;
    public GameObject UI0E;
    public GameObject UI0Space;
    
    // Once the keys are pressed the first time, set the UI elements off
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            UI0A.SetActive(false);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            UI0D.SetActive(false);
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            UI0S.SetActive(false);
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            UI0W.SetActive(false);
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UI0Q.SetActive(false);
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            UI0E.SetActive(false);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UI0Space.SetActive(false);
        }
        
    }
}
