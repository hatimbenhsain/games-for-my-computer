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
    // Start is called before the first frame update
    void Start()
    {
        cutCheckers=GetComponentsInChildren<CutChecker>();
        detachable=false;
        trigger=false;
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
                detachable=true;
                organScript.enabled=true;
                FindObjectOfType<GameController>().skinDetached=true;
                detached=true;
                gameObject.tag="Organ";
                GetComponent<Rigidbody>().isKinematic=false;
                GetComponent<Rigidbody>().useGravity=true;
                for(int i=cutCheckers.Length-1;i<=0;i--){
                    Destroy(cutCheckers[i]);
                }
            }
        }
        trigger=false;
    }
}
