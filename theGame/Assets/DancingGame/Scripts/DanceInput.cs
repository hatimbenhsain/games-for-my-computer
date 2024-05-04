using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DanceInput : MonoBehaviour
{
    public Animator dancingBoy; //Dancing Boy is attached to player, in the Animator are Layers, all layers set to 1 for overlapping
    public Animator dancingBoy2;
    public Image Q;
    public Image W;
    public Image A;
    public Image S;
    public Image D;
    public Image E;
    public Image Space;

    [SerializeField] private Color QColor;
    [SerializeField] private Color WColor;
    [SerializeField] private Color AColor;
    [SerializeField] private Color SColor;
    [SerializeField] private Color DColor;
    [SerializeField] private Color EColor;
    [SerializeField] private Color SPACEColor;
    
    [SerializeField] private Color pressColor;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //left arm wiggle is the isolated animation, same as all the rest, but all layers are at 1
            dancingBoy.Play("LeftArmWiggle");
            dancingBoy2.Play("LeftArmWiggle");
            if (A.color == AColor)
            {
                A.color = pressColor;
            }else
            {
                A.color = AColor;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
                A.color = pressColor;
        }
        else
        {
            A.color = AColor;
        }
        //------------------------------
        if (Input.GetKeyDown(KeyCode.D))
        {
            dancingBoy.Play("RightArmWiggle");
            dancingBoy2.Play("RightArmWiggle");
        }        
        
        if (Input.GetKey(KeyCode.D))
        {
            D.color = pressColor;
        }
        else
        {
            D.color = DColor;
        }
        //--------------------------------
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            dancingBoy.Play("leftlegwiggle");
            dancingBoy2.Play("leftlegwiggle");
        }        
        if (Input.GetKey(KeyCode.S))
        {
            S.color = pressColor;
        }
        else
        {
            S.color = SColor;
        }
        //------------------------------
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            dancingBoy.Play("Rightlegwiggle");
            dancingBoy2.Play("Rightlegwiggle");
        }        
        if (Input.GetKey(KeyCode.W))
        {
            W.color = pressColor;
        }
        else
        {
            W.color = WColor;
        }
        //------------------------------
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            dancingBoy.Play("MiddleWiggle");
            dancingBoy2.Play("MiddleWiggle");
        }        
        if (Input.GetKey(KeyCode.Q))
        {
            Q.color = pressColor;
        }
        else
        {
            Q.color = QColor;
        }
        //----------------------------
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            dancingBoy.Play("dropit");
            dancingBoy2.Play("dropit");
        }        
        if (Input.GetKey(KeyCode.E))
        {
            E.color = pressColor;
        }
        else
        {
            E.color = EColor;
        }
        //----------------------------
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dancingBoy.Play("wiggleit");
            dancingBoy2.Play("wiggleit");
        }        
        if (Input.GetKey(KeyCode.Space))
        {
            Space.color = pressColor;
        }
        else
        {
            Space.color = SPACEColor;
        }
    }
    
}
