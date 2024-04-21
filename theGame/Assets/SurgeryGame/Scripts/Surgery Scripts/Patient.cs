using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Patient : MonoBehaviour
{
    private SurgeryController surgeryController;

    public GameObject[] colliders;

    public float cutOffset=0.05f; //y value for cut gameobject so that it looks above the skin

    public GameObject[] organsToPutIn;
    public GameObject[] organsToRemove;

    public List<GameObject> organsInside;
    private bool organsReplaced=false;

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

    bool CheckOrgans(){     //Check if organs are replaced correctly
        bool removed=true;
        foreach(GameObject g in organsInside){
            if(organsToRemove.Contains(g)){
                removed=false;
            }
        }
        if(!removed){
            organsReplaced=false;
            return false;
        }
        bool inserted=true;
        foreach(GameObject g in organsToPutIn){
            if(!organsInside.Contains(g)){
                inserted=false;
            }
        }
        if(!inserted){
            organsReplaced=false;
            return false;
        }
        if(!organsReplaced){
            SurgeryController sc=FindObjectOfType<SurgeryController>();
            sc.TriggerFeedbackMessage(1);
            sc.ChangeMood();            
            FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue("SurgeryTuto3");
        }
        organsReplaced=true;
        return true;
    }

    public void AddOrgan(GameObject g){
        if(!organsInside.Contains(g)){
            organsInside.Add(g);
        }
        CheckOrgans();
    }

    public void RemoveOrgan(GameObject g){
        while(organsInside.Contains(g)){
            organsInside.Remove(g);
        }
        CheckOrgans();
    }
}
