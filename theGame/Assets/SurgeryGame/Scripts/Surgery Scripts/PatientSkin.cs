using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Behavior for the detachable patient skin
public class PatientSkin : MonoBehaviour
{
    private CutChecker[] cutCheckers;
    public bool trigger;

    public OrganScript organScript;

    public bool detachable;
    private bool detached=false;

    private AudioSource audioSource;
    public AudioClip bigSquish;

    void Start()
    {
        cutCheckers=GetComponentsInChildren<CutChecker>();
        detachable=false;
        trigger=false;
        audioSource=GetComponent<AudioSource>();
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
        if(detachable && !detached && Input.GetMouseButtonUp(0)){
            audioSource.clip=bigSquish;
            audioSource.loop=false;
            audioSource.Play();
            detachable=true;
            //Activating the organScript so the patient skin is treated like a normal organ now
            organScript.enabled=true;
            FindObjectOfType<SurgeryController>().skinDetached=true;

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
        }

        //Resetting trigger
        trigger=false;
    }
}
