using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;
using Yarn.Unity;

//Behavior for the detachable patient skin
public class PatientSkin : MonoBehaviour
{
    private CutChecker[] cutCheckers;
    public bool trigger;

    public OrganScript organScript;

    public bool detachable;
    private bool detached=false;
    private bool reattached=false;

    private AudioSource audioSource;
    public AudioClip bigSquish;

    private int cutCounter;
    private float cutTimer;
    public int minimumCuts=4;
    public float minimumCutTime=5f;

    private bool isCutting=false;

    // Time inbetween mouse presses before this is considered a new cut
    private float cuttingTimeOut=0;
    public float cuttingTimeOutLength=0.2f;

    private int stitchCounter=0;
    public int minimumStitches=3;

    public Material detachedMaterial;
    public Material normalMaterial;

    public GameObject bed;

    public bool inBed=true;

    private SurgeryController surgeryController;

    void Start()
    {
        cutCheckers=GetComponentsInChildren<CutChecker>();
        detachable=false;
        trigger=false;
        audioSource=GetComponent<AudioSource>();
        cutCounter=0;
        cutTimer=0f;

        surgeryController=FindObjectOfType<SurgeryController>();
    }

    void Update()
    {
        //trigger is called by CutChecker scripts
        if(trigger){
            //Checking if every CutChecker has been triggered
            bool complete=true;
            for(int i=0;i<cutCheckers.Length;i++){
                if(!cutCheckers[i].triggered){
                    complete=false;
                    break;
                }
            }
            //What happens if every CutChecker has been triggered
            if(complete){
                detachable=true;
            }
        }

        //Only detach once the mouse button isn't pressed
        if(detachable && !detached && !Input.GetMouseButton(0)){
            Detach();
        }

        if(!reattached && stitchCounter>=minimumStitches && !Input.GetMouseButton(0)){
            Reattach();
        }

        //Resetting trigger
        trigger=false;

        cuttingTimeOut+=Time.deltaTime;
        if(cuttingTimeOut>cuttingTimeOutLength){
            isCutting=false;
        }else if(isCutting){
            cutTimer+=Time.deltaTime;
        }

        //make skin detachable if cut time is superior to timer or cut enough times
        if(cutTimer>=minimumCutTime || cutCounter>=minimumCuts){
            detachable=true;
        }

        if(organScript.enabled && organScript.held){
            stitchCounter=0;
            foreach(Transform t in transform.transform){
                if(t.tag=="Stitch"){
                    Destroy(t.gameObject);
                }
            }
        }
    }

    public void Detach(){
        audioSource.clip=bigSquish;
        audioSource.loop=false;
        audioSource.Play();
        detachable=true;
        //Activating the organScript so the patient skin is treated like a normal organ now
        organScript.enabled=true;

        
        surgeryController.skinDetached=true;
        surgeryController.ChangeMood();

        detached=true;
        gameObject.tag="Organ";
        GetComponent<Rigidbody>().isKinematic=false;
        GetComponent<Rigidbody>().useGravity=true;
        //Destroying cut checkers
        for(int i=cutCheckers.Length-1;i>=0;i--){
            Destroy(cutCheckers[i].gameObject);
        }
        //Destroying body and colliders of cuts so there's less things to process
        GameObject[] cuts=GameObject.FindGameObjectsWithTag("Cut");
        for(int i=cuts.Length-1;i>=0;i--){
            Destroy(cuts[i].GetComponent<BoxCollider>());
            Destroy(cuts[i].GetComponent<Rigidbody>());
        }

        GetComponentInChildren<Renderer>().material=detachedMaterial;
        
        surgeryController.TriggerFeedbackMessage(0);

        FindObjectOfType<DialogueRunner>().StartDialogue("SurgeryTuto2");
    }

    void Reattach(){
        GetComponentInChildren<Renderer>().material=normalMaterial;
        reattached=true;
        GetComponent<Rigidbody>().isKinematic=true;
        GetComponent<Rigidbody>().useGravity=false;
        gameObject.tag="Patient Skin";
        organScript.enabled=false;
        transform.parent=FindObjectOfType<Patient>().transform;
        surgeryController.ChangeMood();
        surgeryController.beeperButton.GetComponent<Animator>().SetTrigger("in");
        Debug.Log("set trigger");

        surgeryController.TriggerFeedbackMessage(2);

        FindObjectOfType<DialogueRunner>().StartDialogue("SurgeryTuto4");
    }

    public void IsCutting(){
        if(!isCutting){
            cutCounter+=1;
        }
        isCutting=true;
        cuttingTimeOut=0;
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject==bed){
            inBed=true;
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject==bed){
            inBed=false;
        }
    }

    public void Stitch(){
        stitchCounter+=1;
    }
}
