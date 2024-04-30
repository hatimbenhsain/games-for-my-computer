using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class LevelSkip : MonoBehaviour
{
    public string destinationScene;

    public float transitionTime;

    public Animator transition;
    public bool setSpawnIndex = false;
    public int spawnIndex = 0;
    public StarterAssets.ThirdPersonController.CharacterState characterState;
    public StarterAssets.ThirdPersonController.CharacterState targetCharacterState;
    public bool playTransformAnimation = false;

    public bool playRain=false;

    private void Start()
    {
        //transition.SetTrigger("Start");
    }

    // 1 is post dancing game, 2 post surgery game, 3 post compliment, 4 post museum
    void Update()
    {
        if (setSpawnIndex)
        {
            // set the location index
            SpawnDataHolder.spawnLocationIndex = spawnIndex;
            SpawnDataHolder.characterState = characterState;
            SpawnDataHolder.targetCharacterState = targetCharacterState;
        }
        SpawnDataHolder.playTransformAnimation = playTransformAnimation;
        SpawnDataHolder.playRain = playRain;

        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("O pressed");
            StartCoroutine(LoadLevelCoroutine(destinationScene));
        }
    }

    [YarnCommand]
    public void LoadLevel(string destination="")
    {
        if(destination==""){
            destination=destinationScene;
        }
        StartCoroutine(LoadLevelCoroutine(destination));
    }



    IEnumerator LoadLevelCoroutine(string levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);

    }
}
