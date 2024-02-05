using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
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

        Cursor.visible=true;

        tool=null;
    }

    // Update is called once per frame
    void Update()
    {
        hoveredOrgan=null;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;

        int layerMask = 1 << 2;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        if (Physics.Raycast(ray, out hitData, 1000) && hitData.transform.gameObject.tag=="Organ"){
            hoveredOrgan=hitData.transform.gameObject.GetComponent<OrganScript>();
        }

        
        RaycastHit hit;
        bool b;
        
        if(Input.GetMouseButtonDown(0)){
            if (tool==null && Physics.Raycast(ray, out hitData, 1000) && hitData.transform.gameObject.tag=="Tool"){
                tool=hitData.transform.gameObject;
                tool.GetComponent<Rigidbody>().isKinematic=true;
                tool.layer=2;
                tool.transform.rotation=Quaternion.Euler(Vector3.zero);
                hand.SetActive(false);
            }else if(tool==null){
                if(heldOrgan==null && hoveredOrgan!=null){
                    hoveredOrgan.held=true;
                    heldOrgan=hoveredOrgan;
                    ChangeMood(1);
                }else if(heldOrgan!=null){
                    heldOrgan.held=false;
                    //heldOrgan.transform.localPosition=heldOrgan.targetPosition;
                    heldOrgan=null;
                    //ChangeMood(1);
                }
            }else{
                b=toolTable.GetComponent<Collider>().Raycast(ray, out hit,100f);
                if(b){
                    tool.GetComponent<Rigidbody>().isKinematic=false;
                    tool.layer=1;
                    hand.SetActive(true);
                    tool=null;
                }
            }
        }
        if(Input.GetMouseButton(0)){
            if (tool!=null && tool.name=="Scalpel" && !skinDetached && Physics.Raycast(ray, out hitData, 1000) && hitData.transform.gameObject.tag=="Patient Skin"){
                Collider skin=hitData.transform.gameObject.GetComponent<Collider>();
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

        
        b=targetPlane.Raycast(ray, out hit,100f);

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
        
    }

    void ChangeMood(int m){
        lights[mood].SetActive(false);
        mood=(mood+1)%songs.Length;
        musicPlayer.Stop();
        musicPlayer.clip=songs[mood];
        musicPlayer.Play();
        if(mood==3){
            cam.GetComponent<Animator>().speed=104f*0.25f/60f;
        }else if(mood==0){
            cam.GetComponent<Animator>().speed=114f*0.5f/60f;
        }else if(mood==1){
            cam.GetComponent<Animator>().speed=170*0.5f/60f;
        }else if(mood==2){
            cam.GetComponent<Animator>().speed=120*0.5f/60f;
        }
        lights[mood].SetActive(true);
    }
}
