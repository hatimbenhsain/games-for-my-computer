using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

//This class checks if this part of the patient skin has been cut, there's a lot of them on the skin.
public class CutChecker : MonoBehaviour
{
    public bool triggered;
    private bool prevTriggered;
    void Start()
    {
        triggered=false;
        prevTriggered=false;
    }

    void Update()
    {
        if(triggered && prevTriggered==false){
            //GetComponent<MeshRenderer>().enabled=false;            
        }
        prevTriggered=triggered;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag=="Cut"){
            triggered=true;
            transform.parent.gameObject.GetComponent<PatientSkin>().trigger=true;
        }
    }
}
