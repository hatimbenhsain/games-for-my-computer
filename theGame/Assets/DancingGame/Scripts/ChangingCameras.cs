using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Collections;

public class ChangingCameras : MonoBehaviour
{
   // private IEnumerator coroutine; //
    
    public CinemachineFreeLook cam1; // Nour
    public CinemachineFreeLook cam2; // DJ
    public CinemachineFreeLook cam3; // Crush
    public CinemachineFreeLook cam4; // Outside

    public GameObject OutsideCoupleUI;

    private int cameraIndex=0;
    private CinemachineFreeLook[] cameras;
    void Start()
    {
        cameras=new CinemachineFreeLook[]{cam1,cam2,cam3,cam4};
        //StartCoroutine(changeCameras()); //co routine for smooth wait time
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Return)){
            ChangeCamera();
        }
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
            cam4.Priority = 10;
            
            yield return new WaitForSeconds(5);
            OutsideCoupleUI.SetActive(true);
            
            yield return new WaitForSeconds(10);
            OutsideCoupleUI.SetActive(false);
            
            yield return new WaitForSeconds(1);
            cam4.Priority = 8;
            cam1.Priority = 10;
        }
    }

    public void ChangeCamera(int index=-1){
        if(index==-1){
            index=cameraIndex+1;
        }
        cameraIndex=index%cameras.Length;
        foreach(CinemachineFreeLook cam in cameras){
            cam.Priority=8;
        }
        cameras[cameraIndex%cameras.Length].Priority=10;
    }
}
