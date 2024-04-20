using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour
{
    private SurgeryController surgeryController;

    public GameObject[] colliders;

    public float cutOffset=0.05f; //y value for cut gameobject so that it looks above the skin

    void Start()
    {
        surgeryController=FindObjectOfType<SurgeryController>();
    }

    void Update()
    {
        if(surgeryController.tool!=null && surgeryController.tool.name=="Thread"){
            foreach(GameObject c in colliders){
                c.SetActive(true);
            }
        }else{
            foreach(GameObject c in colliders){
                c.SetActive(false);
            }
        }
    }
}
