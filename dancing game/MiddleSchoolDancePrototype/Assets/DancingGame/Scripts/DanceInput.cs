using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceInput : MonoBehaviour
{
    public Animator dancingBoy; //Dancing Boy is attached to player, in the Animator are Layers, all layers set to 1 for overlapping

    public AudioSource Clap; //everytime you press a key "clap" happens, but it's too quiet


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //left arm wiggle is the isolated animation, same as all the rest, but all layers are at 1
            dancingBoy.Play("LeftArmWiggle");
            Clap.Play();
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            dancingBoy.Play("RightArmWiggle");
            Clap.Play();
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            dancingBoy.Play("leftlegwiggle");
            Clap.Play();
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            dancingBoy.Play("Rightlegwiggle");
            Clap.Play();
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            dancingBoy.Play("MiddleWiggle");
            Clap.Play();
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            dancingBoy.Play("dropit");
            Clap.Play();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dancingBoy.Play("wiggleit");
            Clap.Play();
        }
    }
    
}
