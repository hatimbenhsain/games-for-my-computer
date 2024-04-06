using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using Yarn.Unity;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public GameObject complimentPrefab;
    public Transform complimentTransform;
    private GameObject complimentInstance;

    public bool inComplimentGame;

    public float inTime = 4.0f;
    public float outTime = 4.0f;
    public float outDialogueTime = 4.0f;
    public CinemachineVirtualCamera playerCamera;
    public Transform playerCameraRoot; // The target position and rotation for the player
    public Transform targetTransform; // The target position and rotation for the camera
    public Vector3 targetShoulderOffset = Vector3.zero; // target shoulder offset for head zoom in
    public float targetCameraDistance = 0f; // target camera dist for head zoom in
    public bool isTransitioning = false;
    public float transitionSpeed = 5.0f;
    private Vector3 previousPosition;
    private Quaternion previousRotation;
    private Vector3 initialMousePosition;

    private DialogueRunner dialogueRunner;

    private ThirdPersonController playerScript;

    private Animator animator;

    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera=Camera.main;
        inComplimentGame=false;
        playerScript=FindObjectOfType<ThirdPersonController>();
        dialogueRunner=FindObjectOfType<DialogueRunner>();
        animator = GetComponentInChildren<Animator>();
        //isTransitioning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTransitioning)
        {
            playerCamera.Follow = null;
            playerCamera.LookAt = null;
            // Interpolate camera's position
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, targetTransform.position, transitionSpeed * Time.deltaTime);


            // Interpolate camera's rotation
            playerCamera.transform.rotation = Quaternion.Lerp(playerCamera.transform.rotation, targetTransform.rotation, transitionSpeed * Time.deltaTime);
        }
    }

    [YarnCommand]
    public void ComplimentStart()
    {
        //  FREEZE PLAYER

        if (!inComplimentGame)
        {
            inComplimentGame = true;
            dialogueRunner.Stop();
            animator.SetTrigger("ComplimentIn");

            //playerCamera.LookAt = null;
            isTransitioning = true;
            initialMousePosition = Input.mousePosition;
            StartCoroutine(ComplimentStartCoroutine());
        }


    }

    public void ComplimentStartOperations()
    {

        //INSTANTIATE COMPLIMENT
        //SWITCH CAMERA
        //  CHANGE CURSOR SETTINGS
        //  SET COMPLIMENT WORDS/LEVEL
        //END DIALOGUE

        mainCamera.enabled = false;
        complimentInstance = Instantiate(complimentPrefab, complimentTransform);

    }


    public void ComplimentEnd(){
        if (inComplimentGame)
        {
            animator.SetTrigger("ComplimentOut");
            playerCamera.Follow = playerCameraRoot;
            playerCamera.LookAt = playerCameraRoot;
            isTransitioning = false;
            StartCoroutine(ComplimentEndCoroutine());
            StartCoroutine(ComplimentEndDialogueCoroutine());

        }
    } 

    public void ComplimentEndOperations()
    {
        //RESUME DIALOGUE

            Destroy(complimentInstance);
            mainCamera.enabled = true;

    }

    public void ComplimentEndDialogueOperations()
    {
        //RESUME DIALOGUE

        dialogueRunner.Stop();
        dialogueRunner.StartDialogue(playerScript.npcTalkingTo.talkToNode);
        inComplimentGame = false;



    }


    private IEnumerator ComplimentStartCoroutine()
    {
        // wait
        yield return new WaitForSeconds(inTime);
        ComplimentStartOperations();

    }
    private IEnumerator ComplimentEndCoroutine()
    {
        // wait
        yield return new WaitForSeconds(outTime);
        ComplimentEndOperations();
    }
    private IEnumerator ComplimentEndDialogueCoroutine()
    {
        // wait
        yield return new WaitForSeconds(outDialogueTime);
        ComplimentEndDialogueOperations();
    }
}
