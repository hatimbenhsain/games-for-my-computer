using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    public GameObject complimentPrefab;
    public Transform complimentTransform;
    private GameObject complimentInstance;

    private bool inComplimentGame;

    private DialogueRunner dialogueRunner;

    private ThirdPersonController playerScript;

    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera=Camera.main;
        inComplimentGame=false;
        playerScript=FindObjectOfType<ThirdPersonController>();
        dialogueRunner=FindObjectOfType<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    [YarnCommand]
    public void ComplimentStart()
    {
        //  FREEZE PLAYER
        //INSTANTIATE COMPLIMENT
        //SWITCH CAMERA
        //  CHANGE CURSOR SETTINGS
        //  SET COMPLIMENT WORDS/LEVEL
        //END DIALOGUE
        if(!inComplimentGame){
            mainCamera.enabled=false;
            complimentInstance=Instantiate(complimentPrefab,complimentTransform);
            inComplimentGame=true;
            dialogueRunner.Stop();
        }
    }

    public void ComplimentEnd(){
        //RESUME DIALOGUE
        if(inComplimentGame){
            Destroy(complimentInstance);
            mainCamera.enabled=true;
            inComplimentGame=false;
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue(playerScript.npcTalkingTo.talkToNode);
        }
    } 
}
