using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Collections;

public class ChangingCameras : MonoBehaviour
{
   // private IEnumerator coroutine; //
    
    public CinemachineFreeLook cam1;

    public CinemachineFreeLook cam2;

    public CinemachineFreeLook cam3;
   
    void Start()
    {
        StartCoroutine(changeCameras()); //co routine for smooth wait time
    }
    IEnumerator changeCameras()
    {
        while (true) //while (true) allows for the coroutine to continue in a loop as it's always true, probably could be better?
        {
            yield return new WaitForSeconds(10); //wait for 10 seconds before switching to new camera

            cam1.Priority = 8; //cameras are always on, we use priority to smooth transition between, over set.active. Higher priority # means that camera is now visible
            cam2.Priority = 10;

            yield return new WaitForSeconds(10);

            cam2.Priority = 8;
            cam3.Priority = 10;

            yield return new WaitForSeconds(10);

            cam3.Priority = 8;
            cam1.Priority = 10;
        }
    }
}
