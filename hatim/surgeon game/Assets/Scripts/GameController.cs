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
    

    private 
    void Start()
    {
        cam=FindObjectOfType<Camera>();
        heldOrgan=null;
        targetPlane=targetPlaneObject.GetComponent<MeshCollider>();
        skinDetached=false;
        audioSource=GetComponent<AudioSource>();
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

        
        if(Input.GetMouseButtonDown(0)){
            if(heldOrgan==null && hoveredOrgan!=null){
                hoveredOrgan.held=true;
                heldOrgan=hoveredOrgan;
            }else if(heldOrgan!=null){
                heldOrgan.held=false;
                //heldOrgan.transform.localPosition=heldOrgan.targetPosition;
                heldOrgan=null;
            }
        }
        if(Input.GetMouseButton(0)){
            if (!skinDetached && Physics.Raycast(ray, out hitData, 1000) && hitData.transform.gameObject.tag=="Patient Skin"){
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

        if(heldOrgan!=null){
            RaycastHit hit;
            if(targetPlane.Raycast(ray, out hit,100f)){
                Vector3 point=ray.GetPoint(hit.distance);
                heldOrgan.targetPosition=point;
            }
        }
    }
}
