using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public GameObject alienText;
    public Animator artMove;
    public String artMoveName;

    //this function is called from Alien Interact Script. What happens here is what happens 
    //here is what happens when you press E to interact with aliens
    public void Interact()
    {
    
        alienText.SetActive(true);
        artMove.Play(artMoveName);
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<SphereCollider>().enabled = true;
    }
    
}
