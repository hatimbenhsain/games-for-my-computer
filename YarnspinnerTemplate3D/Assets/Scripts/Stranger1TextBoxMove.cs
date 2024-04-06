using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;//need this to be able to use yarn command

public class Stranger1TextBoxMove : MonoBehaviour
{
    public GameObject objectToMove1;
    public GameObject objectToMove2;
    public float targetYCoordinate = 900f;

    void Start()
    {
       
    }

    // Custom method to handle button click

    [YarnCommand]//hey yarn pay attention to this cause its a command
    public void MoveBox1()//name of the command 
    {
        objectToMove1.transform.position = new Vector3(objectToMove1.transform.position.x, targetYCoordinate, objectToMove1.transform.position.z);//what we want to have happen when the command is called
        objectToMove2.transform.position = new Vector3(objectToMove2.transform.position.x, targetYCoordinate, objectToMove2.transform.position.z);//what we want to have happen when the command is called
    }
}