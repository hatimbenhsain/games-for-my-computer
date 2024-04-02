using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ComplimentManager : MonoBehaviour
{
    private InMemoryVariableStorage variableStorage;
    private string compliment;
    
    void Start()
    {
        variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)){

            ComplimentEnd();
        }
    }


    void ComplimentEnd(){
        GenerateCompliment();
        variableStorage.SetValue("$compliment",compliment);
        FindObjectOfType<GameManager>().ComplimentEnd();
    }

    //Function for generating compliment text from words picked
    void GenerateCompliment(){
        //MAKE COMPLIMENT FROM TEMPLATES
        compliment="Your stench is so beautiful it makes me want to cry.";
    }

}
