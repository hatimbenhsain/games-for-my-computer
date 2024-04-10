using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class BossSphere : MonoBehaviour
{
    public float speed=2f;
    private float lifeLength=10f;
    private float lifeTime=0f;
    
    private CanvasScript canvasScript;

    void Start()
    {
        //transform.parent=transform.parent.parent.parent;
        canvasScript=FindObjectOfType<CanvasScript>();
    }

    void Update()
    {
        Vector3 pos=transform.position;
        transform.position=pos+transform.forward*Time.deltaTime*speed;      
        lifeTime+=Time.deltaTime;
        if(lifeTime>lifeLength){
            Destroy(gameObject);
        } 
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag=="Player"){
            canvasScript.WhiteScreen();
            Debug.Log("white screen");
        }else{
            Debug.Log(other.gameObject);
        }
    }
}
