using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using Yarn.Unity;

public class DeliveryGame : MonoBehaviour
{
    public GameObject cardboardBoxPrefab;
    private Camera mainCamera;
    private Canvas canvas;
    public GameObject package;
    private int packageIndex;

    private int packageNumber;  //how many packages can exist?

    private bool limitedPackages=true; //do we stop delivering packages at a certain point?
              // maybe the answer should be no but at some point it's just pizza or whatever


    void Start()
    {
        Cursor.visible=true;
        Cursor.lockState=CursorLockMode.None;
        mainCamera=Camera.main;
        canvas=FindObjectOfType<Canvas>();
        package=null;
        FindObjectOfType<ThirdPersonController>().canMoveInDialogue=true;
        FindObjectOfType<ThirdPersonController>().maxHeight=50f;
        packageIndex=0;
        packageNumber=1000;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible=true;
        Cursor.lockState=CursorLockMode.None;
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.4f);
        if(package!=null && package.GetComponentInChildren<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name=="cardboardBoxIdle"){
            Destroy(package.GetComponentInChildren<Camera>().gameObject);
            mainCamera.gameObject.SetActive(true);
            canvas.gameObject.SetActive(true);
            package.GetComponentInChildren<Rigidbody>().isKinematic=false;
            package=null;
        }
        if(package==null && Input.GetKeyDown(KeyCode.P)){
            DropPackage();
        }
    }

    void DropPackage(){
        if(!limitedPackages || packageIndex<packageNumber){
            package=Instantiate(cardboardBoxPrefab);
            Vector3 pos=new Vector3(Random.Range(-5f,5f),package.transform.position.y,Random.Range(-5f,5f));
            package.transform.position=pos;
            mainCamera.gameObject.SetActive(false);
            canvas.gameObject.SetActive(false);
            package.GetComponentInChildren<Rigidbody>().isKinematic=true;
            package.GetComponentInChildren<Package>().index=packageIndex;
            packageIndex++;
            packageNumber=package.GetComponentInChildren<Package>().items.Length;
        }
    }

    public void Bark(string node="Bark1"){
        DialogueRunner dr=FindObjectOfType<DialogueRunner>();
        dr.Stop();
        FindObjectOfType<DialogueRunner>().StartDialogue(node);
    }
}
