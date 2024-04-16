using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour
{
    private SurgeryController surgeryController;

    public GameObject[] colliders;

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
