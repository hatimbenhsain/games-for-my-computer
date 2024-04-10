using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SurgeryController : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera cam;
    public GameObject sphere;

    public GameObject targetPlaneObject;

    private MeshCollider targetPlane;
    public OrganScript hoveredOrgan;
    public OrganScript heldOrgan;

    public GameObject cutPrefab;

    public bool skinDetached;

    public AudioSource audioSource;
    public AudioClip zipping;

    public GameObject hand;
    public GameObject tool;
    public GameObject toolTable;

    private int mood=0;
    public AudioClip[] songs;
    public GameObject[] lights;
    public AudioSource musicPlayer;
    void Start()
    {
        cam=FindObjectOfType<Camera>();
        heldOrgan=null;
        targetPlane=targetPlaneObject.GetComponent<MeshCollider>();
        skinDetached=false;
        audioSource=GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible=true;

        tool=null;
    }

    void Update()
    {
        hoveredOrgan=null;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;

        int layerMask = 1 << 2;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        //Finding organ we're hovering on
        if (Physics.Raycast(ray, out hitData, 1000) && hitData.transform.gameObject.tag=="Organ"){
            hoveredOrgan=hitData.transform.gameObject.GetComponent<OrganScript>();
        }

        
        RaycastHit hit;
        bool b;
        
        //New left click
        if(Input.GetMouseButtonDown(0)){
            // Picking up tool
            if (tool==null && Physics.Raycast(ray, out hitData, 1000) && hitData.transform.gameObject.tag=="Tool"){
                tool=hitData.transform.gameObject;
                tool.GetComponent<Rigidbody>().isKinematic=true;
                tool.layer=2;
                tool.transform.rotation=Quaternion.Euler(Vector3.zero);
                hand.SetActive(false);
            }else if(tool==null){
                //Picking up organ
                if(heldOrgan==null && hoveredOrgan!=null){
                    hoveredOrgan.held=true;
                    heldOrgan=hoveredOrgan;
                    ChangeMood(1);
                }
                //Dropping organ
                else if(heldOrgan!=null){
                    heldOrgan.held=false;
                    heldOrgan=null;
                }
            }else{
                //Dropping tool if above table
                b=toolTable.GetComponent<Collider>().Raycast(ray, out hit,100f);
                if(b){
                    tool.GetComponent<Rigidbody>().isKinematic=false;
                    tool.layer=1;
                    hand.SetActive(true);
                    tool=null;
                }
            }
        }

        //Current left click
        if(Input.GetMouseButton(0)){
            //Cutting skin if holding scalpel
            if (tool!=null && tool.name=="Scalpel" && !skinDetached && Physics.Raycast(ray, out hitData, 1000) && hitData.transform.gameObject.tag=="Patient Skin"){
                Collider skin=hitData.transform.gameObject.GetComponent<Collider>();
                //Creating cut marks
                GameObject cut=Instantiate(cutPrefab,hitData.point,cutPrefab.transform.rotation,skin.transform);
                cut.transform.localPosition=new Vector3(cut.transform.localPosition.x,0.05f,cut.transform.localPosition.z);
                if(!audioSource.isPlaying){
                    audioSource.clip=zipping;
                    audioSource.loop=true;
                    audioSource.Play();
                }
            }
        }else{
            audioSource.Stop();
        }

        //targetPlane is the collider of the plane in the scene where the current picked up objects should be
        b=targetPlane.Raycast(ray, out hit,100f);

        //Moving picked up object to plane
        if(b){
            Vector3 point=ray.GetPoint(hit.distance);
            if(tool!=null){
                tool.transform.localPosition=point;
            }else{
                hand.transform.localPosition=point;
            }
            if(heldOrgan!=null){
                heldOrgan.targetPosition=point;
            }
        }

        //Restarting scene
        if(Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }

    //Function to change the mood of the scene
    void ChangeMood(int m){

        //Deactivate the current lights
        lights[mood].SetActive(false);
        

        //Change music
        mood=(mood+1)%songs.Length;
        musicPlayer.Stop();
        musicPlayer.clip=songs[mood];
        musicPlayer.Play();

        //Change the camera zoom speed
        if(mood==3){
            cam.GetComponent<Animator>().speed=104f*0.25f/60f;
        }else if(mood==0){
            cam.GetComponent<Animator>().speed=114f*0.5f/60f;
        }else if(mood==1){
            cam.GetComponent<Animator>().speed=170*0.5f/60f;
        }else if(mood==2){
            cam.GetComponent<Animator>().speed=120*0.5f/60f;
        }

        //Activate new lights
        lights[mood].SetActive(true);
        
    }
}
