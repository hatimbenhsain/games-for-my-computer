using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using Yarn.Unity;
using Cinemachine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameObject[] complimentPrefab;
    public Transform complimentTransform;
    private GameObject complimentInstance;

    private int complimentValue;

    public bool inComplimentGame;

    public float inTime = 4.0f;
    public float outTime = 4.0f;
    public float outCameraTime = 0.1f;
    public float outDialogueTime = 4.0f;
    public CinemachineVirtualCamera playerCamera;
    public Transform playerCameraRoot; // The target position and rotation for the player
    public Transform targetTransform; // The target position and rotation for the camera
    public Transform transformCamera; // camera location for NPC
    public Vector3 targetShoulderOffset = Vector3.zero; // target shoulder offset for head zoom in
    public float targetCameraDistance = 0f; // target camera dist for head zoom in
    public bool isTransitioning = false;
    public bool isEndTransitioning = false;
    public float transitionSpeed = 5.0f;
    public float outTransitionSpeed = 1.0f;
    private Vector3 previousPosition;
    private Quaternion previousRotation;
    private Vector3 initialMousePosition;

    private DialogueRunner dialogueRunner;

    private ThirdPersonController playerScript;

    private Animator animator;
    private Vector3 endCameraPosition;
    private Quaternion endCameraRotation;

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

        complimentValue=0;
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
        if (isEndTransitioning)
        {
            // Interpolate camera's position
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, endCameraPosition, outTransitionSpeed * Time.deltaTime);


            // Interpolate camera's rotation
            playerCamera.transform.rotation = Quaternion.Lerp(playerCamera.transform.rotation, endCameraRotation, outTransitionSpeed * Time.deltaTime);
            float lerpProgress = Mathf.Abs(Vector3.Distance(playerCamera.transform.position, endCameraPosition));
            Debug.Log(lerpProgress);
            if (lerpProgress < 0.1)
            {
                Debug.Log("end lerping");
                playerCamera.Follow = playerCameraRoot;
                playerCamera.LookAt = playerCameraRoot;
                isEndTransitioning = false;
            }
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
        complimentInstance = Instantiate(complimentPrefab[complimentValue%complimentPrefab.Length],
         complimentTransform);
        complimentValue+=1;

    }


    public void ComplimentEnd(){
        if (inComplimentGame)
        {
            animator.SetTrigger("ComplimentOut");
            isTransitioning = false;
            //UnityEngine.Cursor.lockState = CursorLockMode.Locked;


            StartCoroutine(ComplimentEndCoroutine());
            StartCoroutine(ComplimentEndCameraCoroutine());
            StartCoroutine(ComplimentEndDialogueCoroutine());

        }
    } 

    public void ComplimentEndOperations()
    {

        endCameraPosition = playerCamera.transform.position;
        endCameraRotation = playerCamera.transform.rotation;
        // set camera back to head position
        playerCamera.Follow = null;
        playerCamera.LookAt = null;
        playerCamera.transform.position = targetTransform.position;
        playerCamera.transform.rotation = targetTransform.rotation;
        isEndTransitioning = true;

    }

    public void ComplimentEndDialogueOperations()
    {
        //RESUME DIALOGUE

        dialogueRunner.Stop();
        dialogueRunner.StartDialogue(playerScript.npcTalkingTo.talkToNode);
        inComplimentGame = false;
       // UnityEngine.Cursor.lockState = CursorLockMode.None;



    }

    public void ComplimentEndCameraOperations()
    {
        //RESUME DIALOGUE
        mainCamera.enabled = true;
        Destroy(complimentInstance);


        //record end camera position
        playerCamera.Follow = playerCameraRoot;
        playerCamera.LookAt = playerCameraRoot;

    }


    private IEnumerator ComplimentStartCoroutine()
    {
        // wait
        yield return new WaitForSeconds(inTime);
        ComplimentStartOperations();

    }
    private IEnumerator ComplimentEndCameraCoroutine()
    {
        // wait
        yield return new WaitForSeconds(outCameraTime);
        ComplimentEndCameraOperations();
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
