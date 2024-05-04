using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

//  This script is for making an object appear/disappear depending on what state the player is currently in
public class ObjectAppear : MonoBehaviour
{
    private ThirdPersonController thirdPersonController;

    public ThirdPersonController.CharacterState[] statesToAppearIn;   //states that are okay to appear in

    void Start()
    {
        thirdPersonController=FindObjectOfType<ThirdPersonController>();
    }

    void Update()
    {
        
    }
}
