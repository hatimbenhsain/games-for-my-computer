using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueTrigger : MonoBehaviour
{
    public string nodeToCall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag=="Player"){
            FindObjectOfType<DialogueRunner>().StartDialogue(nodeToCall);
        }
    }
}
