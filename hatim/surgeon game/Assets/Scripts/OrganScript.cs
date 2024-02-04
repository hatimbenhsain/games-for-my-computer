using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrganScript : MonoBehaviour
{
    public bool held;
    private bool prevHeld;
    private Rigidbody body;

    public Vector3 targetPosition;
    private float lerpValue;
    // Start is called before the first frame update
    void Start()
    {
        held=false;
        body=GetComponent<Rigidbody>();
        lerpValue=20f;
    }

    // Update is called once per frame
    void Update()
    {
        if(prevHeld!=held && held){
            body.useGravity=false;
        }else if(prevHeld!=held){
            body.useGravity=true;
        }

        if(held){
            transform.localPosition=Vector3.Lerp(transform.localPosition,targetPosition,lerpValue*Time.deltaTime);
            Quaternion targetRotation=Quaternion.Euler(0f,0f,0f);
            transform.rotation=Quaternion.Lerp(transform.rotation,targetRotation,lerpValue*Time.deltaTime);
        }

        prevHeld=held;
    }
}
