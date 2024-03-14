using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject cardboardBoxPrefab;
    private Camera mainCamera;
    private Canvas canvas;
    public GameObject package;


    void Start()
    {
        Cursor.visible=true;
        mainCamera=Camera.main;
        canvas=FindObjectOfType<Canvas>();
        package=null;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible=true;
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
        package=Instantiate(cardboardBoxPrefab);
        Vector3 pos=new Vector3(Random.Range(-5f,5f),package.transform.position.y,Random.Range(-5f,5f));
        package.transform.position=pos;
        mainCamera.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
        package.GetComponentInChildren<Rigidbody>().isKinematic=true;
    }
}
