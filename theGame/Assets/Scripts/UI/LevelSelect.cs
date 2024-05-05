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

    public void LoadDance()
    {
        menuLoader.destinationScene = "DanceTest";
        menuLoader.setSpawnIndex = false;
        menuLoader.playTransformAnimation = false;
        menuLoader.transition = transition;
        menuLoader.playRain = false;
        menuLoader.PlayGame();

    }

    public void LoadPostDance()
    {
        // Post dancing game
        menuLoader.destinationScene = destinationScene;
        menuLoader.setSpawnIndex = true;
        menuLoader.playTransformAnimation = false;
        menuLoader.loaderCharacterState = StarterAssets.ThirdPersonController.CharacterState.Fish;
        menuLoader.targetCharacterState = StarterAssets.ThirdPersonController.CharacterState.Fish;
        menuLoader.transition = transition;
        menuLoader.playRain = true;
        menuLoader.spawnIndex = 1;
        menuLoader.PlayGame();
    }
    public void LoadSurgery()
    {
        menuLoader.destinationScene = "SurgeryIntroScene";
        menuLoader.setSpawnIndex = false;
        menuLoader.playTransformAnimation = false;
        menuLoader.transition = transition;
        menuLoader.playRain = false;
        menuLoader.PlayGame();

    }

    public void LoadPostSurgery()
    {
        // Post surgery
        menuLoader.destinationScene = destinationScene;
        menuLoader.setSpawnIndex = true;
        menuLoader.playTransformAnimation = false;
        menuLoader.loaderCharacterState = StarterAssets.ThirdPersonController.CharacterState.Leg;
        menuLoader.targetCharacterState = StarterAssets.ThirdPersonController.CharacterState.Leg;
        menuLoader.transition = transition;
        menuLoader.playRain = true;
        menuLoader.spawnIndex = 2;
        menuLoader.PlayGame();
    }

    public void LoadMuseum()
    {
        menuLoader.destinationScene = "MuseumGame";
        menuLoader.setSpawnIndex = false;
        menuLoader.playTransformAnimation = false;
        menuLoader.transition = transition;
        menuLoader.playRain = false;
        menuLoader.PlayGame();
    }

    public void LoadPostCompliment()
    {
        // Post Compliment
        // TODO: 
        menuLoader.destinationScene = destinationScene;
        menuLoader.setSpawnIndex = true;
        menuLoader.playTransformAnimation = false;
        menuLoader.loaderCharacterState = StarterAssets.ThirdPersonController.CharacterState.Leg;
        menuLoader.targetCharacterState = StarterAssets.ThirdPersonController.CharacterState.Wing;
        menuLoader.transition = transition;
        menuLoader.playRain = true;
        menuLoader.spawnIndex = 3;
        menuLoader.PlayGame();
    }


    public void LoadPostMuseum()
    {
        // Post Museum
        menuLoader.destinationScene = destinationScene;
        menuLoader.setSpawnIndex = true;
        menuLoader.playTransformAnimation = false;
        menuLoader.loaderCharacterState = StarterAssets.ThirdPersonController.CharacterState.Wing;
        menuLoader.targetCharacterState = StarterAssets.ThirdPersonController.CharacterState.Rocket;
        menuLoader.transition = transition;
        menuLoader.playRain = true;
        menuLoader.spawnIndex = 4;
        menuLoader.PlayGame();
    }

    public void LoadBoss()
    {
        menuLoader.destinationScene = "BossCutScene";
        menuLoader.setSpawnIndex = false;
        menuLoader.playTransformAnimation = false;
        menuLoader.transition = transition;
        menuLoader.playRain = false;
        menuLoader.PlayGame();
    }

    public void LoadCredit()
    {
        menuLoader.destinationScene = "EndingCredits";
        menuLoader.setSpawnIndex = false;
        menuLoader.playTransformAnimation = false;
        menuLoader.transition = transition;
        menuLoader.playRain = false;
        menuLoader.PlayGame();
    }
}
