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

    private RectTransform rectTransform1;
    private RectTransform rectTransform2;

    void Start()
    {
        rectTransform1=objectToMove1.GetComponent<RectTransform>();
        rectTransform2=objectToMove2.GetComponent<RectTransform>();
    }

    // Custom method to handle button click

    [YarnCommand]//hey yarn pay attention to this cause its a command
    public void MoveBox1()//name of the command 
    {
        //objectToMove1.transform.position = new Vector3(objectToMove1.transform.position.x, targetYCoordinate, objectToMove1.transform.position.z);//what we want to have happen when the command is called
        //objectToMove2.transform.position = new Vector3(objectToMove2.transform.position.x, targetYCoordinate, objectToMove2.transform.position.z);//what we want to have happen when the command is called
        rectTransform1.anchoredPosition=new Vector2(rectTransform1.anchoredPosition.x,targetYCoordinate);
        rectTransform2.anchoredPosition=new Vector2(rectTransform2.anchoredPosition.x,targetYCoordinate);
    }
}