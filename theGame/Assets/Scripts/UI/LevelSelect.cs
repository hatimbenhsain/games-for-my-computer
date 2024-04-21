using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class LevelSelect : MonoBehaviour
{
    public MainMenu menuLoader;
    public string destinationScene;
    public Animator transition;
    // Start is called before the first frame update
    void Start()
    {
        //menuLoader = GameObject.Find("MainMenu").GetComponent<MainMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void LoacCheckPoint1()
    {
        // Post dancing game
        menuLoader.destinationScene = destinationScene;
        menuLoader.setSpawnIndex = true;
        menuLoader.playTransformAnimation = true;
        menuLoader.loaderCharacterState = StarterAssets.ThirdPersonController.CharacterState.Leg;
        menuLoader.targetCharacterState = StarterAssets.ThirdPersonController.CharacterState.Fish;
        menuLoader.transition = transition;
        menuLoader.spawnIndex = 1;
        menuLoader.PlayGame();
    }

    public void LoacCheckPoint2()
    {
        // Post surgery
        menuLoader.destinationScene = destinationScene;
        menuLoader.setSpawnIndex = true;
        menuLoader.playTransformAnimation = true;
        menuLoader.loaderCharacterState = StarterAssets.ThirdPersonController.CharacterState.Fish;
        menuLoader.targetCharacterState = StarterAssets.ThirdPersonController.CharacterState.Leg;
        menuLoader.transition = transition;
        menuLoader.spawnIndex = 2;
        menuLoader.PlayGame();
    }

    public void LoacCheckPoint3()
    {
        // Post Compliment
        // TODO: 
        menuLoader.destinationScene = destinationScene;
        menuLoader.setSpawnIndex = true;
        menuLoader.playTransformAnimation = true;
        menuLoader.loaderCharacterState = StarterAssets.ThirdPersonController.CharacterState.Leg;
        menuLoader.targetCharacterState = StarterAssets.ThirdPersonController.CharacterState.Wing;
        menuLoader.transition = transition;
        menuLoader.spawnIndex = 3;
        menuLoader.PlayGame();
    }

    public void LoadCheckPoint4()
    {
        // Post Museum
        menuLoader.destinationScene = destinationScene;
        menuLoader.setSpawnIndex = true;
        menuLoader.playTransformAnimation = true;
        menuLoader.loaderCharacterState = StarterAssets.ThirdPersonController.CharacterState.Wing;
        menuLoader.targetCharacterState = StarterAssets.ThirdPersonController.CharacterState.Rocket;
        menuLoader.transition = transition;
        menuLoader.spawnIndex = 4;
        menuLoader.PlayGame();
    }
}
