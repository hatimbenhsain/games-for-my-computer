using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class CutChecker : MonoBehaviour
{
    // Start is called before the first frame update
    public bool triggered;
    private bool prevTriggered;
    void Start()
    {
        triggered=false;
        prevTriggered=false;
    }

    // Update is called once per frame
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
