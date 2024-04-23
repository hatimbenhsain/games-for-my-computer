using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

//Organs behavior script
public class OrganScript : MonoBehaviour
{
    public bool held;
    private bool prevHeld;
    private Rigidbody body;

    public Vector3 targetPosition;
    private float lerpValue;

    public AudioClip squish;
    public AudioClip longSquish;
    public AudioClip thud;

    private Animator animator;

    private AudioSource audioSource;
    private ParticleSystem particleSystem;

    public float meshOffset=0.05f;

    void Start()
    {
        held=false;
        body=GetComponent<Rigidbody>();
        lerpValue=20f;
        audioSource=GetComponent<AudioSource>();
        animator=GetComponentInChildren<Animator>();
        particleSystem=GetComponentInChildren<ParticleSystem>();
    }


    void Update()
    {
        //What happens when the organ just started being held
        if(prevHeld!=held && held){
            body.useGravity=false;
            audioSource.clip=squish;
            audioSource.loop=false;
            audioSource.Play();
            audioSource.volume=1f;
            if(animator!=null){
                animator.SetBool("held",true);
            }
            if(particleSystem!=null){
                particleSystem.Play();
            }
        }
        //What happens when the organ just stopped being held
        else if(prevHeld!=held){
            body.useGravity=true;
            if(animator!=null){
                animator.SetBool("held",false);
            }
            if(particleSystem!=null){
                particleSystem.Stop();
            }
        }

        if(held){
            //Moving organ to target (mouse) + resetting rotation
            if(GetComponentInChildren<MeshRenderer>()!=null || GetComponentInChildren<SkinnedMeshRenderer>()!=null){
                targetPosition=new Vector3(targetPosition.x,targetPosition.y-meshOffset,targetPosition.z);
            }
            transform.localPosition=Vector3.Lerp(transform.localPosition,targetPosition,lerpValue*Time.deltaTime);
            Quaternion targetRotation=Quaternion.Euler(0f,0f,0f);
            transform.rotation=Quaternion.Lerp(transform.rotation,targetRotation,lerpValue*Time.deltaTime);

            if(!audioSource.isPlaying){
                audioSource.clip=longSquish;
                audioSource.loop=true;
                audioSource.Play();
                audioSource.volume=1f;
            }
        }

        prevHeld=held;
    }

    private void OnCollisionEnter(Collision other) {
        if(!held){
            //Play a sound effect when organ collides with something with varying intensity depending on velocity
            float mag=body.velocity.magnitude;
            if(mag>0.5f){
                audioSource.clip=thud;
                audioSource.loop=false;
                audioSource.Play();
                audioSource.volume=Mathf.Min(mag,1f);
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.name=="InsideContainerTrigger"){
            FindObjectOfType<Patient>().AddOrgan(gameObject);
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.name=="InsideContainerTrigger"){
            FindObjectOfType<Patient>().RemoveOrgan(gameObject);
        }
    }
}
