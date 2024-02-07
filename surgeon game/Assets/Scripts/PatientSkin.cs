using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientSkin : MonoBehaviour
{
    private CutChecker[] cutCheckers;
    public bool trigger;

    public OrganScript organScript;

    public bool detachable;
    private bool detached=false;

    private AudioSource audioSource;
    public AudioClip bigSquish;
    // Start is called before the first frame update
    void Start()
    {
        cutCheckers=GetComponentsInChildren<CutChecker>();
        detachable=false;
        trigger=false;
        audioSource=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(trigger){
            bool complete=true;
            for(int i=0;i<cutCheckers.Length;i++){
                if(!cutCheckers[i].triggered){
                    complete=false;
                    break;
                }
            }
            if(complete && !detached && Input.GetMouseButtonUp(0)){
                audioSource.clip=bigSquish;
                audioSource.loop=false;
                audioSource.Play();
                detachable=true;
                organScript.enabled=true;
                FindObjectOfType<GameController>().skinDetached=true;
                detached=true;
                gameObject.tag="Organ";
                GetComponent<Rigidbody>().isKinematic=false;
                GetComponent<Rigidbody>().useGravity=true;
                for(int i=cutCheckers.Length-1;i>=0;i--){
                    Destroy(cutCheckers[i].gameObject);
                }
                GameObject[] cuts=GameObject.FindGameObjectsWithTag("Cut");
                for(int i=cuts.Length-1;i>=0;i--){
                    Destroy(cuts[i].GetComponent<BoxCollider>());
                    Destroy(cuts[i].GetComponent<Rigidbody>());
                }
            }
        }
        trigger=false;
    }
}
