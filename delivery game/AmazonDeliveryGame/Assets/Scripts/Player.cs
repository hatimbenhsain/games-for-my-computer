using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject CanvasObject;
    
    // Start is called before the first frame update
    void Start()
    {
        CanvasObject=FindObjectOfType<Canvas>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag=="Boss" && other.gameObject.GetComponent<Boss>().currentState==Boss.State.Charging){
            CanvasObject.GetComponent<CanvasScript>().RedScreen();
        }else if(other.gameObject.tag=="BossSphere"){
            CanvasObject.GetComponent<CanvasScript>().WhiteScreen();
            Debug.Log("white screen");
        }
    }
}
