using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string destinationScene;

    public float transitionTime;

    public Animator transition;
    public bool setSpawnIndex = false;
    public bool playTransformAnimation = false;
    public int spawnIndex = 0;
    public StarterAssets.ThirdPersonController.CharacterState loaderCharacterState;
    public StarterAssets.ThirdPersonController.CharacterState targetCharacterState;

    // Start is called before the first frame update
    public void PlayGame()
    {
        StartCoroutine(LoadLevel(destinationScene));
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    IEnumerator LoadLevel(string levelIndex)
    {
        if (setSpawnIndex)
        {
            // set the location index
            SpawnDataHolder.spawnLocationIndex = spawnIndex;
            SpawnDataHolder.characterState = loaderCharacterState;
            SpawnDataHolder.targetCharacterState = targetCharacterState;
        }
        SpawnDataHolder.playTransformAnimation = playTransformAnimation;
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);

    }
}
