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

    private void Start()
    {
        transition.Play("New State", 0, 0f);
        if (Time.timeScale == 0)
        {
            Debug.LogWarning("Time.timeScale is 0, setting it to 1.");
            Time.timeScale = 1;
        }
    }

    // TODO: Load location, transformation, can enter building or not, npc state
    // Start is called before the first frame update
    public void PlayGame()
    {
        Debug.Log("PlayCoroutine");
        StartCoroutine(LoadLevel(destinationScene));
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    IEnumerator LoadLevel(string levelIndex)
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
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
        Debug.Log("Load Level");
        SceneManager.LoadScene(levelIndex);

    }
}
