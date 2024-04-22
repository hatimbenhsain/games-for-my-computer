using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class CutsceneLoader : MonoBehaviour
{
    public string destinationScene;

    public float transitionTime;

    public Animator transition;


    private void Start()
    {
        //transition.SetTrigger("Start");
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
