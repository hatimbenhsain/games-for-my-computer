using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneLoader : MonoBehaviour
{
    public string destinationScene;

    public float transitionTime;

    public Animator transition;


    private void Start()
    {
        //transition.SetTrigger("Start");
    }

    public void LoadLevel()
    {
        StartCoroutine(LoadLevel(destinationScene));
    }


    IEnumerator LoadLevel(string levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);

    }
}
