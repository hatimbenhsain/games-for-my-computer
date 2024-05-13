using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using Yarn.Unity;
using Cinemachine;
using UnityEngine.UIElements;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject[] complimentPrefab;
    public Transform complimentTransform;
    private GameObject complimentInstance;

    private int complimentValue;
    public int maxComplimentValue=6;

    public bool inComplimentGame;

    public float inTime = 4.0f;
    public float outTime = 4.0f;
    public float outCameraTime = 0.1f;
    public float outDialogueTime = 4.0f;
    private CinemachineVirtualCamera playerCamera;
    private Transform playerCameraRoot; // The target position and rotation for the player
    private Transform targetTransform; // The target position and rotation for the camera
    private Transform transformCamera; // camera location for NPC
    public Vector3 targetShoulderOffset = Vector3.zero; // target shoulder offset for head zoom in
    public float targetCameraDistance = 0f; // target camera dist for head zoom in
    public bool isTransitioning = false;
    public bool isEndTransitioning = false;
    public float transitionSpeed = 5.0f;
    public float outTransitionSpeed = 1.0f;
    public Light directionalLight;

    private DialogueRunner dialogueRunner;

    private ThirdPersonController playerScript;

    private Animator animator;
    private Vector3 endCameraPosition;
    private Quaternion endCameraRotation;

    private Camera mainCamera;

    private bool complimentMetamorphosisHappened=false;

    public ObjectAppear[] objectsToAppear;
    private float originalFixedDeltaTime;
    private Transform respawnTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.Find("PlayerFollowCamera")?.GetComponent<CinemachineVirtualCamera>();
        playerCameraRoot = GameObject.Find("PlayerCameraRoot")?.GetComponent<Transform>();
        targetTransform = GameObject.Find("LerpTargetCamera")?.GetComponent<Transform>();
        mainCamera =Camera.main;
        inComplimentGame=false;
        playerScript=FindObjectOfType<ThirdPersonController>();
        dialogueRunner=FindObjectOfType<DialogueRunner>();
        animator = GetComponentInChildren<Animator>();
        //isTransitioning = true;
        complimentValue=0;
        originalFixedDeltaTime = Time.fixedDeltaTime;
        objectsToAppear =FindObjectsOfType<ObjectAppear>();
        AppearObjects();
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

        if(!complimentMetamorphosisHappened && !inComplimentGame && !dialogueRunner.IsDialogueRunning && !isTransitioning && complimentValue>=maxComplimentValue){
            StartCoroutine("PostComplimentEvent");
            complimentMetamorphosisHappened=true;
            Debug.Log("post");
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
            Time.fixedDeltaTime = 0.005f;
            //playerCamera.LookAt = null;
            isTransitioning = true;
            StartCoroutine(ComplimentStartCoroutine());
        }


    }

    public void ComplimentStartOperations()
    {

        //INSTANTIATE COMPLIMENT
        //SWITCH CAMERA
        // Disable Directional Light
        //  CHANGE CURSOR SETTINGS
        //  SET COMPLIMENT WORDS/LEVEL
        //END DIALOGUE
        directionalLight.enabled = false;
        mainCamera.enabled = false;
        complimentInstance = Instantiate(complimentPrefab[complimentValue%complimentPrefab.Length],
         complimentTransform);
        complimentValue+=1;
        GameObject.FindObjectOfType<InMemoryVariableStorage>().SetValue("$complimentValue",complimentValue.ToString());
    }


    public void ComplimentEnd(){
        if (inComplimentGame)
        {
            animator.SetTrigger("ComplimentOut");
            isTransitioning = false;
            //UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            Time.fixedDeltaTime = originalFixedDeltaTime;

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
        animator.SetTrigger("Reset");
        // UnityEngine.Cursor.lockState = CursorLockMode.None;



    }

    public void ComplimentEndCameraOperations()
    {
        //RESUME DIALOGUE
        directionalLight.enabled = true;
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

    //Co-routine for metamorphosis and rain that takes place after every compliment has been paid
    private IEnumerator PostComplimentEvent(){
        yield return new WaitForSeconds(2f);
        while(dialogueRunner.IsDialogueRunning){
            yield return new WaitForSeconds(2f);
        }
        dialogueRunner.StartDialogue("Rain3");
    }

    public void PlayerChangedState(){
        AppearObjects();
    }

    void AppearObjects(){
        foreach(ObjectAppear o in objectsToAppear){
            if(o.statesToAppearIn.Contains(playerScript.currentState)){
                o.gameObject.SetActive(true);
            }else{
                o.gameObject.SetActive(false);
            }
        }
    }

    public void Respawn(){
        playerScript.Respawn(respawnTransform);
    }

    public void SetRespawnTransform(Transform t){
        respawnTransform=t;
    }
}
