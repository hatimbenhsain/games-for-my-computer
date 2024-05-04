using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DanceInput1 : MonoBehaviour
{
    public Animator dancingBoy; //Dancing Boy is attached to player, in the Animator are Layers, all layers set to 1 for overlapping

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //left arm wiggle is the isolated animation, same as all the rest, but all layers are at 1
            dancingBoy.Play("LeftArmWiggle");
        }
           

        //------------------------------
        if (Input.GetKeyDown(KeyCode.D))
        {
            dancingBoy.Play("RightArmWiggle");
            
        }        
        

        //--------------------------------
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            dancingBoy.Play("leftlegwiggle");
         
        }        

        //------------------------------
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            dancingBoy.Play("Rightlegwiggle");
       
        }        

        //------------------------------
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            dancingBoy.Play("MiddleWiggle");
       
        }        
        //----------------------------
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            dancingBoy.Play("dropit");
            
        }        

        //----------------------------
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dancingBoy.Play("wiggleit");
         
        }        
    }
    
}
