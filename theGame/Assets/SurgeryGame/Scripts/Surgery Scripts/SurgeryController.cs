using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class SurgeryController : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera cam;
    public GameObject sphere;

    public GameObject targetPlaneObject;

    private MeshCollider targetPlane;
    public OrganScript hoveredOrgan;
    public OrganScript heldOrgan;

    public GameObject cutPrefab;
    public GameObject stitchPrefab;

    public bool skinDetached;

    public AudioSource audioSource;
    public AudioClip zipping;
    public AudioClip[] stitchSounds;
    public AudioClip[] tableWheelSounds;

    public GameObject hand;
    public GameObject tool;
    public GameObject toolTable;

    private int mood=0;
    public AudioClip[] songs;
    public GameObject[] lights;
    public AudioSource musicPlayer;

    private bool isStitching=false;
    int stitchType=-1; //-1 if outside body 0 if on body 1 if on skin

    public GameObject[] thingsToMoveOut;

    public GameObject[] thingsToMoveIn;

    private bool movingPatients=false;
    private bool movingPatientsIn=false;
    public float movingTime=2f; //how long it takes to move patients in/out
    private float movingTimer=0f;
    public float movingDistance=5f;

    public GameObject[] patientSetList;
    public int patientIndex=0;

    private bool curtainsOpen;

    public GameObject curtains;
    public AudioClip curtainOpenClip;
    public AudioClip curtainCloseClip;

    private SurgeryMusic surgeryMusic;
    private DialogueRunner dialogueRunner;

    public GameObject beeperButton;

    void Start()
    {
        cam=FindObjectOfType<Camera>();
        heldOrgan=null;
        targetPlane=targetPlaneObject.GetComponent<MeshCollider>();
        skinDetached=false;
        audioSource=GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible=false;

        tool=null;

        curtainsOpen=false;

        //OpenCurtains();

        surgeryMusic=FindObjectOfType<SurgeryMusic>();
        dialogueRunner=FindObjectOfType<DialogueRunner>();
        songs=surgeryMusic.songs;

        dialogueRunner.StartDialogue("Patient"+(patientIndex+1).ToString()+"Intro");
    }

    void Update()
    {
        hoveredOrgan=null;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;

        int layerMask = 1 << 2;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        //Finding organ we're hovering on
        if (Physics.Raycast(ray, out hitData, 1000) && hitData.transform.gameObject.tag=="Organ"){
            hoveredOrgan=hitData.transform.gameObject.GetComponent<OrganScript>();
        }

        
        RaycastHit hit;
        bool b;
        
        //New left click
        if(Input.GetMouseButtonDown(0)){
            // Picking up tool
            if (tool==null && Physics.Raycast(ray, out hitData, 1000) && hitData.transform.gameObject.tag=="Tool"){
                tool=hitData.transform.gameObject;
                tool.GetComponent<Rigidbody>().isKinematic=true;
                tool.layer=2;
                tool.transform.rotation=Quaternion.Euler(Vector3.zero);
                hand.SetActive(false);
            }else if(tool==null){
                //Picking up organ
                if(heldOrgan==null && hoveredOrgan!=null){
                    hoveredOrgan.held=true;
                    heldOrgan=hoveredOrgan;
                    ChangeMood();
                }
                //Dropping organ
                else if(heldOrgan!=null){
                    heldOrgan.held=false;
                    heldOrgan=null;
                }
            }else{
                //Dropping tool if above table
                b=toolTable.GetComponent<Collider>().Raycast(ray, out hit,100f);
                if(b){
                    tool.GetComponent<Rigidbody>().isKinematic=false;
                    tool.layer=1;
                    hand.SetActive(true);
                    tool=null;
                }
            }
        }

        //Current left click
        if(Input.GetMouseButton(0)){
            //Cutting skin if holding scalpel
            if (tool!=null && tool.name=="Scalpel" && !skinDetached && Physics.Raycast(ray, out hitData, 1000) && hitData.transform.gameObject.tag=="Patient Skin"){
                CutSkin(hitData);
            }
            if (tool!=null && tool.name=="Thread" && skinDetached){
                Stitch(hitData);
            }else{
                isStitching=false;
            }
        }else{
            audioSource.Stop();
            isStitching=false;
        }

        //targetPlane is the collider of the plane in the scene where the current picked up objects should be
        b=targetPlane.Raycast(ray, out hit,100f);

        //Moving picked up object to plane
        if(b){
            Vector3 point=ray.GetPoint(hit.distance);
            if(tool!=null){
                tool.transform.localPosition=point;
            }else{
                hand.transform.localPosition=point;
            }
            if(heldOrgan!=null){
                heldOrgan.targetPosition=point;
            }
        }

        //Restarting scene
        if(Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(movingPatients){
            float k1=movingTimer/movingTime;
            movingTimer+=Time.deltaTime;
            float k2=movingTimer/movingTime;
            float dif=easeInOutSine(k2)-easeInOutSine(k1);
            foreach(GameObject g in thingsToMoveOut){
                Vector3 p=g.transform.position;
                p.x-=dif*movingDistance;
                g.transform.position=p;
            }

            if(movingPatientsIn){
                foreach(GameObject g in thingsToMoveIn){
                    Vector3 p=g.transform.position;
                    p.x-=dif*movingDistance;
                    g.transform.position=p;
                }
            }
            
            if(movingTimer>movingTime){
                movingPatients=false;
                movingTimer=0f;
                foreach(GameObject g in thingsToMoveOut){
                    g.SetActive(false);
                }
                if(movingPatientsIn){
                    foreach(GameObject g in thingsToMoveIn){
                        foreach(OrganScript o in g.GetComponentsInChildren<OrganScript>()){
                            if(o.gameObject.tag!="Patient Skin"){
                                o.GetComponent<Rigidbody>().isKinematic=false;
                                o.GetComponent<Rigidbody>().useGravity=true;
                            }
                        }
                        foreach(Collider c in g.GetComponentsInChildren<Collider>()){
                            c.enabled=true;
                        }
                    }
                    movingPatientsIn=false;
                    skinDetached=false;
                    stitchType=-1;
                    audioSource.clip=tableWheelSounds[Random.Range(0,tableWheelSounds.Length)];
                    audioSource.loop=false;
                    audioSource.Play();
                }

                patientIndex+=1;
                //OpenCurtains();
            }
        }

        if(Input.GetKeyDown(KeyCode.Return)){
            SwitchPatients();
        }

    }

    //Function to change the mood of the scene
    public void ChangeMood(int m=-1){

        //Deactivate the current lights
        lights[mood].SetActive(false);
        

        //Change music
        if(m==-1){
            mood=(mood+1)%songs.Length;
        }else{
            mood=(m)%songs.Length;
        }
        // musicPlayer.Stop();
        // musicPlayer.clip=songs[mood];
        // musicPlayer.Play();

        surgeryMusic.CrossFade(mood);

        //Change the camera zoom speed
        if(mood==3){
            cam.GetComponent<Animator>().speed=104f*0.25f/60f;
        }else if(mood==0){
            cam.GetComponent<Animator>().speed=114f*0.5f/60f;
        }else if(mood==1){
            cam.GetComponent<Animator>().speed=170*0.5f/60f;
        }else if(mood==2){
            cam.GetComponent<Animator>().speed=120*0.5f/60f;
        }

        //Activate new lights
        lights[mood].SetActive(true);
        
    }

    void CutSkin(RaycastHit hitData){
        Collider skin=hitData.transform.gameObject.GetComponent<Collider>();
        //Creating cut marks
        GameObject cut=Instantiate(cutPrefab,hitData.point,cutPrefab.transform.rotation,skin.transform);
        cut.transform.localPosition=new Vector3(cut.transform.localPosition.x,0.05f,cut.transform.localPosition.z);
        if(!audioSource.isPlaying){
            audioSource.clip=zipping;
            audioSource.loop=true;
            audioSource.Play();
        }
        skin.gameObject.GetComponent<PatientSkin>().IsCutting();
    }

    void Stitch(RaycastHit hitData){
        if(FindObjectOfType<PatientSkin>().inBed){
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            int prevStitchType=stitchType;
            Transform par=FindObjectOfType<Patient>().gameObject.transform; //parent of stitch object
            if(Physics.Raycast(ray, out hitData, 1000) && hitData.transform.gameObject.name=="patientSkin"){
                stitchType=1;
                par=hitData.transform;
            }else if(hitData.transform.gameObject.tag=="Patient Body"){
                stitchType=0;
            }else{
                stitchType=-1;
            }
            if(!isStitching){
                if(stitchType!=-1){
                    isStitching=true;
                }
            }
            if(isStitching){
                GameObject stitch=Instantiate(stitchPrefab,hitData.point,stitchPrefab.transform.rotation,
                par);
                stitch.transform.position=new Vector3(stitch.transform.position.x,
                0.07f,stitch.transform.position.z);
                if(!audioSource.isPlaying){
                    audioSource.clip=stitchSounds[Random.Range(0,stitchSounds.Length)];
                    audioSource.loop=true;
                    audioSource.Play();
                }
            }
            //The stitch is validated once it has crossed between the skin and the body     
            if(stitchType!=prevStitchType && prevStitchType!=-1 && stitchType!=-1){
                FindObjectOfType<PatientSkin>().Stitch();
            }
        }
    }

    public void MovePatientOut(){
        ChangeMood();
        if(patientIndex<patientSetList.Length){
            movingPatients=true;
            thingsToMoveOut=new GameObject[] {patientSetList[patientIndex]};
            foreach(GameObject g in thingsToMoveOut){
                foreach(Collider c in g.GetComponentsInChildren<Collider>()){
                    c.enabled=false;
                }
                foreach(Rigidbody rb in g.GetComponentsInChildren<Rigidbody>()){
                    rb.useGravity=false;
                    rb.isKinematic=true;
                }
            }
        }
    }

    public void MovePatientIn(){
        if(patientIndex<patientSetList.Length-1){
            movingPatientsIn=true;
            thingsToMoveIn=new GameObject[] {patientSetList[patientIndex+1]};
            foreach(GameObject g in thingsToMoveIn){
                g.SetActive(true);
                foreach(OrganScript o in g.GetComponentsInChildren<OrganScript>()){
                    if(o.gameObject.tag!="Patient Skin"){
                        o.GetComponent<Rigidbody>().isKinematic=true;
                        o.GetComponent<Rigidbody>().useGravity=false;
                    }
                }
                foreach(Collider c in g.GetComponentsInChildren<Collider>()){
                    c.enabled=false;
                }
            }
            audioSource.clip=tableWheelSounds[Random.Range(0,tableWheelSounds.Length)];
            audioSource.loop=false;
            audioSource.Play();
        }
    }

    float easeInOutSine(float x){
        return -(Mathf.Cos(Mathf.PI*x)-1)/2;
    }

    [YarnCommand]
    public void OpenCurtains(){
        curtains.GetComponent<Animator>().SetTrigger("curtainsOpen");
        curtains.GetComponent<AudioSource>().clip=curtainOpenClip;
        curtains.GetComponent<AudioSource>().Play();
    }

    [YarnCommand]
    public void CloseCurtains(){
        curtains.GetComponent<Animator>().SetTrigger("curtainsClose");
        curtains.GetComponent<AudioSource>().clip=curtainCloseClip;
        curtains.GetComponent<AudioSource>().Play();
    }

    public void SwitchPatients(){
        if(!movingPatients && !movingPatientsIn){
            CloseCurtains();
            MovePatientOut();
            MovePatientIn();
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue("Patient"+(patientIndex+2).ToString()+"Intro");
            beeperButton.GetComponent<Animator>().SetTrigger("out");
            EventSystem.current.SetSelectedGameObject(null); 
        }
    }
}
