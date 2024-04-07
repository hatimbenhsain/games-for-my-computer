using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Start()
    {
        //transition.SetTrigger("Start");
    }

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

        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("O pressed");
            StartCoroutine(LoadLevel(destinationScene));
        }
    }


    IEnumerator LoadLevel(string levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);

    }
}
