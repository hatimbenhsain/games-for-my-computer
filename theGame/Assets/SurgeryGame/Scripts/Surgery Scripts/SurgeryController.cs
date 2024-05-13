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

    public GameObject feedbackMessagePrefab;

    private int changeMoodNumber=0;
    public int[] moodIndexArray;

    public int whiteLine1State;   //-1 if never appeared 0 if appearing cut 1 if disappearing cut 2 if appearing stitch 3 if disappearing stitch
    public SpriteRenderer whiteLineRenderer;

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

        whiteLine1State=-1;
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
                if(tool.name=="Scalpel" && whiteLine1State==-1){
                    whiteLine1State=0;
                }else if(tool.name=="Thread" && whiteLine1State<=1 && skinDetached){
                    whiteLine1State=2;
                    whiteLineRenderer.gameObject.GetComponent<Animator>().SetBool("cut",false);
                }
            }else if(tool==null){

            }else if(tool!=null){
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
            if(tool!=null && tool.name=="Scalpel" && !skinDetached && Physics.Raycast(ray, out hitData, 1000) && hitData.transform.gameObject.tag=="Patient Skin"){
                CutSkin(hitData);
                if(whiteLine1State==0){
                    whiteLine1State=1;
                }
            }
            if (tool!=null && tool.name=="Thread" && skinDetached){
                Stitch(hitData);
            }else{
                isStitching=false;
            }

            //Picking up organ
            if(tool==null && heldOrgan==null && hoveredOrgan!=null){
                hoveredOrgan.held=true;
                heldOrgan=hoveredOrgan;
                //ChangeMood();
            }
        }else{
            audioSource.Stop();
            isStitching=false;
        }

        if(Input.GetMouseButtonUp(0)){
            //Dropping organ
            if(heldOrgan!=null){
                heldOrgan.held=false;
                heldOrgan=null;
            }
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

        if(whiteLine1State==0 || whiteLine1State==2){
            Color c=whiteLineRenderer.color;
            whiteLineRenderer.color=new Color(c.r,c.g,c.b,Mathf.Clamp(c.a+Time.deltaTime,0,1));
        }else if(whiteLine1State==1 || whiteLine1State==3){
            Color c=whiteLineRenderer.color;
            whiteLineRenderer.color=new Color(c.r,c.g,c.b,Mathf.Clamp(c.a-Time.deltaTime,0,1));
        }

    }

    //Function to change the mood of the scene
    public void ChangeMood(int m=-1){

        //Deactivate the current lights
        if(changeMoodNumber>=songs.Length){
            lights[moodIndexArray[mood]].SetActive(false);
        }else{
            lights[mood].SetActive(false);
        }

        //Change music
        if(m==-1){
            mood=(mood+1)%songs.Length;
        }else{
            mood=(m)%songs.Length;
        }
        // musicPlayer.Stop();
        // musicPlayer.clip=songs[mood];
        // musicPlayer.Play();

        //m=mood;

        if(m==-1){
            changeMoodNumber++;
            if(changeMoodNumber%songs.Length==0){
                moodIndexArray=new int[songs.Length];
                for(int i=0;i<songs.Length;i++){
                    moodIndexArray[i]=i;
                }
                Shuffle(moodIndexArray);
            }
            if(changeMoodNumber>=songs.Length){
                m=moodIndexArray[mood];
            }else{
                m=mood;
            }
        }else{
            m=mood;
        }

        surgeryMusic.CrossFade(m);

        //Change the camera zoom speed
        if(m==0){
            cam.GetComponent<Animator>().speed=114f*0.5f/60f;
        }else if(m==1){
            cam.GetComponent<Animator>().speed=170*0.5f/60f;
        }else if(m==2){
            cam.GetComponent<Animator>().speed=75*0.5f/60f;
        }else if(m==3){
            cam.GetComponent<Animator>().speed=120*0.25f/60f;
        }else if(m==4){
            cam.GetComponent<Animator>().speed=104*0.5f/60f;
        }else if(m==5){
            cam.GetComponent<Animator>().speed=126*0.5f/60f;
        }else if(m==6){
            cam.GetComponent<Animator>().speed=141*0.5f/60f;
        }

        //Activate new lights
        lights[m].SetActive(true);
        
    }

    void CutSkin(RaycastHit hitData){
        Collider skin=hitData.transform.gameObject.GetComponent<Collider>();
        //Creating cut marks
        GameObject cut=Instantiate(cutPrefab,hitData.point,cutPrefab.transform.rotation,skin.transform);
        float cutOffset=FindObjectOfType<Patient>().cutOffset;
        cut.transform.localPosition=new Vector3(cut.transform.localPosition.x,cutOffset,cut.transform.localPosition.z);
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
            }else if(hitData.transform.gameObject.tag=="Patient Body" || hitData.transform.parent.gameObject.name=="insideContainer"){
                stitchType=0;
            }else{
                stitchType=-1;
            }
            if(!isStitching){
                if(stitchType!=-1){
                    isStitching=true;
                }
            }
            Debug.Log(stitchType);
            Debug.Log(hitData.transform.gameObject);
            if(isStitching){
                GameObject stitch=Instantiate(stitchPrefab,hitData.point,stitchPrefab.transform.rotation,
                par);
                float cutOffset=FindObjectOfType<Patient>().cutOffset;
                stitch.transform.position=new Vector3(stitch.transform.position.x,
                cutOffset+0.01f,stitch.transform.position.z);
                if(!audioSource.isPlaying){
                    audioSource.clip=stitchSounds[Random.Range(0,stitchSounds.Length)];
                    audioSource.loop=true;
                    audioSource.Play();
                }
                if(whiteLine1State==2){
                    whiteLine1State=3;
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
            if(!movingPatientsIn){
                dialogueRunner.StartDialogue("PatientOutro");
            }else{
                dialogueRunner.StartDialogue("Patient"+(patientIndex+2).ToString()+"Intro");
            }
            beeperButton.GetComponent<Animator>().SetTrigger("out");
            EventSystem.current.SetSelectedGameObject(null); 
        }
    }

    public void TriggerFeedbackMessage(int type){   //0: after cutting 1: after organs 2: after stitching
        GameObject fm=Instantiate(feedbackMessagePrefab);
        fm.GetComponent<FeedbackMessage>().SetSprite(type);
    }

    void Shuffle(int[] array){
        System.Random _random = new System.Random();
        int p = array.Length;
        for (int n = p-1; n > 0 ; n--)
        {
            int r = _random.Next(1, n);
            int t = array[r];
            array[r] = array[n];
            array[n] = t;
        }
    }
}
