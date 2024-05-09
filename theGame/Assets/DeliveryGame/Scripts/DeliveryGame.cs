using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class DeliveryGame : MonoBehaviour
{
    public GameObject cardboardBoxPrefab;
    private Camera mainCamera;
    public Canvas canvas;
    public GameObject package;
    private int packageIndex;

    private int packageNumber;  //how many packages can exist?

    private bool limitedPackages=true; //do we stop delivering packages at a certain point?
              // maybe the answer should be no but at some point it's just pizza or whatever

    public GameObject[] reorderWindows;

    public float newFillSpeed=0.2f;
    public ProgressBarGame progressBarGame;

    public Transform[] packagePositions;

    public AudioSource musicSource;

    public AudioClip bowserHeroic;
    public float fadeOutTime=1f;

    void Start()
    {
        Cursor.visible=true;
        Cursor.lockState=CursorLockMode.None;
        mainCamera=Camera.main;
        //canvas=FindObjectOfType<Canvas>();
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
            canvas.enabled=true;
            package.GetComponentInChildren<Rigidbody>().isKinematic=false;
            package=null;
        }
        if(package==null && Input.GetKeyDown(KeyCode.P)){
            //DropPackage();
        }
    }

   public void DropPackage(){
        if(!limitedPackages || packageIndex<packageNumber){
            if(packageIndex==0){
                StartCoroutine(MusicTransition());
            }
            package=Instantiate(cardboardBoxPrefab);
            Vector3 pos=packagePositions[packageIndex].position;
            //pos=new Vector3(Random.Range(-5f,5f),package.transform.position.y,Random.Range(-5f,5f));
            pos=new Vector3(pos.x,package.transform.position.y,pos.z);
            package.transform.position=pos;
            package.transform.rotation=packagePositions[packageIndex].rotation;
            //mainCamera.gameObject.SetActive(false);
            canvas.enabled=false;
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

    [YarnCommand]
    public void Reorder(){
        if(packageIndex<packageNumber && packageIndex-1<reorderWindows.Length){
            reorderWindows[packageIndex-1].SetActive(true);
            progressBarGame.fillSpeed=newFillSpeed;
            musicSource.pitch=musicSource.pitch+0.025f;
        }
    }

    private IEnumerator MusicTransition(){
        float ogVolume=musicSource.volume;
        while(musicSource.volume>0f){
            musicSource.volume=musicSource.volume-0.025f/fadeOutTime;
            yield return new WaitForSeconds(0.025f);
        }
        musicSource.Stop();
        musicSource.clip=bowserHeroic;
        musicSource.volume=ogVolume;
        musicSource.time=0f;
        musicSource.Play();
    }
}
