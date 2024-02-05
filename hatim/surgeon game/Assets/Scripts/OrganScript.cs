using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

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
    // Start is called before the first frame update
    void Start()
    {
        held=false;
        body=GetComponent<Rigidbody>();
        lerpValue=20f;
        audioSource=GetComponent<AudioSource>();
        animator=GetComponentInChildren<Animator>();
        particleSystem=GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
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
        }else if(prevHeld!=held){
            body.useGravity=true;
            if(animator!=null){
                animator.SetBool("held",false);
            }
            if(particleSystem!=null){
                particleSystem.Stop();
            }
        }

        if(held){
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
            float mag=body.velocity.magnitude;
            if(mag>0.5f){
                audioSource.clip=thud;
                audioSource.loop=false;
                audioSource.Play();
                audioSource.volume=Mathf.Min(mag,1f);
            }
        }
    }
}
