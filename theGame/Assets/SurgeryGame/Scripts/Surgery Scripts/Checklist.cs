using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Checklist : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject[] checklists;
    private bool showing=false;

    private SurgeryController surgeryController;
    void Start()
    {
        surgeryController=FindObjectOfType<SurgeryController>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject checklist in checklists){
            checklist.SetActive(false);
        }
        if(showing){
            checklists[surgeryController.patientIndex].SetActive(true);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        showing=true;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        showing=false;
        Debug.Log("pointer exit");
    }
}
