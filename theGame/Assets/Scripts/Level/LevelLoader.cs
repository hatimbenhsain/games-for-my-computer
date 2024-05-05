using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class LevelLoader : MonoBehaviour
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadLevel();
        }
    }

    [YarnCommand]
    public void LoadLevel(string destination="")
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
        if(destination==""){
            destination=destinationScene;
        }
        StartCoroutine(LoadLevelCoroutine(destination));
    }

    [YarnCommand]
    public void LoadLevel1(string destination="")
    {
        LoadLevel(destination);
    }

    IEnumerator LoadLevelCoroutine(string levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);

    }
}
