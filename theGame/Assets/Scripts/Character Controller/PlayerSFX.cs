using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using UnityEngine;
using UnityEngine.AI;

public class PlayerSFX : MonoBehaviour
{
    //public string mainTrackGameObjectName="Nichole-WHub";
    public AudioSource source1; //For short burst 1
    public AudioSource source2; //For short burst 2
    public AudioSource source3; // For falling
    public AudioSource source4; // For running perc
    public AudioSource source5; // For drone 2/rumble
    public AudioSource source6; // For drone 3/rocket perc
    public AudioSource source7; // For running cymbal
    public AudioSource source8; // For running gong
    private AudioSource[] audioSources;

    [Header("Fish")]
    public AudioClip[] fishJumpClips;

    [Tooltip("Additional percussion to play on top of splash sound effect")] 
    public AudioClip[] fishJumpClips2;
    public float fishJump1MaxVolume=1f;
    public float fishJump2MaxVolume=1f;
    public bool fishJump2Variation=false;

    private float lastJumpedTimer=0f;

    public AudioClip[] fishLandClips;
    public AudioClip[] fishLandClips2;
    public float fishLand1MaxVolume=1f;
    public float fishLand2MaxVolume=1f;
    public bool fishLand2Variation=false;

    public float fishLandMinForce=1f;
    public float fishLandMaxForce=10f;
    public float fishLandMinVolume=0.2f;

    [Header("Leg")]
    public AudioClip[] runningClips;
    public AudioClip runningClipCymbal;
    public AudioClip runningClipGong;
    private bool running=false;
    private int runningSpeed=0;
    private float runningVolume=0f;
    private float runningVolume2=0f;
    private float runningVolume3=0f;
    public float runningFadeinSpeed=0.5f;
    public float runningFadeOutSpeed=0.5f;

    public float runningFadeinSpeed2=4f;
    public float runningFadeOutSpeed2=4f;

    public float runningFadeinSpeed3=6f;
    public float runningFadeOutSpeed3=6f;
    public float runningMaxVolume=1f;
    

    [Header("Wing")]
    public AudioClip[] jumpClips;

    [Tooltip("Additional percussion to play on top of jump sound effect")] 
    public AudioClip[] jumpClips2;
    public float jump1MaxVolume=1f;
    public float jump2MaxVolume=1f;
    public bool jump2Variation=false;

    public AudioClip[] landClips;
    public AudioClip[] landClips2;
    public float land1MaxVolume=1f;
    public float land2MaxVolume=1f;
    public float landMinForce=1f;
    public float landMaxForce=10f;
    public float landMinVolume=0.2f;
    public bool land2Variation=false;


    [Header("Rocket")]
    public AudioClip rumbleClip;
    public AudioClip rocketPercClip;    //Rocket percussion track
    private float rocketTimer;
    private bool rocketing=false;
    private float rocketVolume=0f;
    [Tooltip("How fast the rocket volume goes up")]
    public float rocketVolumeSpeed=1f;
    public float rocketVolume2FadeInSpeed=1f;
    public float rocketVolume2FadeOutSpeed=1f;
    private float rocketPercVolume=0f;
    

    [Header("Falling")]
    public AudioClip[] fallingClips;
    public float fallingMaxVolume=1f;
    private bool falling=false;
    private bool prevFalling=false;

    [Header("Misc")]
    public float maxVolume=1f;

    private AudioSource mainTrack;
    void Start()
    {
        audioSources=new AudioSource[]{source1,source2,source3,source4,source5,source6,source7,source8};
        rocketTimer=0f;
        source5.clip=rumbleClip;
        source5.volume=maxVolume*rocketVolume;
        source5.loop=true;
        source6.clip=rocketPercClip;
        source6.volume=0;
        source6.loop=true;
        source6.Play();
        mainTrack=GameObject.FindGameObjectWithTag("Main Track").GetComponent<AudioSource>();

        source4.volume=0f;
        source4.loop=true;
        source4.clip=runningClips[0];
        source4.Play();
        source7.volume=0f;
        source7.loop=true;
        source7.clip=runningClipCymbal;
        source7.Play();
        source8.volume=0f;
        source8.loop=true;
        source8.clip=runningClipGong;
        source8.Play();

        StartCoroutine(SyncTracks());
    }

    void Update()
    {

        lastJumpedTimer+=Time.deltaTime;   

        
        //Rocket Control
        if(rocketing){
            rocketVolume=rocketVolume+rocketVolumeSpeed*Time.deltaTime;
            if(mainTrack!=null && mainTrack.isPlaying){
                rocketPercVolume+=rocketVolume2FadeInSpeed*Time.deltaTime;
            }
        }else{
            rocketVolume=rocketVolume-rocketVolumeSpeed*Time.deltaTime;
            rocketPercVolume-=rocketVolume2FadeOutSpeed*Time.deltaTime;
        }
        rocketVolume=Mathf.Clamp(rocketVolume,0f,1f);
        rocketPercVolume=Mathf.Clamp(rocketPercVolume,0f,1f);
        source5.volume=rocketVolume*maxVolume;
        if(source5.volume==0){
            source5.Stop();
        }else if(!source5.isPlaying){
            source5.Play();
        }
        source6.volume=rocketPercVolume*maxVolume;
        
        prevFalling=falling;

        if(running && mainTrack!=null && mainTrack.isPlaying){
            runningVolume+=runningFadeinSpeed*Time.deltaTime;
            runningVolume2+=runningFadeinSpeed2*Time.deltaTime;
            runningVolume3+=runningFadeinSpeed3*Time.deltaTime;
        }else{
            runningVolume-=runningFadeOutSpeed*Time.deltaTime;
            runningVolume2-=runningFadeOutSpeed2*Time.deltaTime;
            runningVolume3-=runningFadeOutSpeed3*Time.deltaTime;
        }
        runningVolume=Mathf.Clamp(runningVolume,0f,1f);
        runningVolume2=Mathf.Clamp(runningVolume2,0f,1f);
        runningVolume3=Mathf.Clamp(runningVolume3,0f,1f);

        source4.volume=runningVolume*maxVolume*runningMaxVolume;
        source7.volume=runningVolume2*maxVolume*runningMaxVolume;
        source8.volume=runningVolume3*maxVolume*runningMaxVolume;
    }

    public void JumpFish(){
        if(lastJumpedTimer>0.1f){
            PlayRandomClip(fishJumpClips,0,fishJump1MaxVolume,1f,true);
            if(fishJumpClips2.Length>0) PlayRandomClip(fishJumpClips2,1,fishJump2MaxVolume,1f,fishJump2Variation);
            lastJumpedTimer=0f;
        }
        falling=false;
    }

    public void LandFish(float force=1f){
        float v=Mathf.Clamp((Mathf.Abs(force)-fishLandMinForce)/fishLandMaxForce,fishLandMinVolume,1f);
        PlayRandomClip(fishLandClips,0,v*fishLand1MaxVolume,1f,true);
        if(fishLandClips2.Length>0) PlayRandomClip(fishLandClips2,1,v*fishLand2MaxVolume,1f,fishLand2Variation);
    }

    public void Jump(){
        if(lastJumpedTimer>0.1f){
            PlayRandomClip(jumpClips,0,jump1MaxVolume,1f,true);
            if(jumpClips2.Length>0) PlayRandomClip(jumpClips2,1,jump2MaxVolume,1f,jump2Variation);
            lastJumpedTimer=0f;
        }
        falling=false;
    }

    public void Land(float force){
        if(Mathf.Abs(force)>=landMinForce){
            float v=Mathf.Clamp((Mathf.Abs(force)-landMinForce)/landMaxForce,0f,1f);
            int i=Mathf.FloorToInt(v*(landClips.Length-1));
            PlayClip(landClips[i],0,land1MaxVolume,1f);
            if(landClips2.Length>0) PlayRandomClip(landClips2,1,v*land2MaxVolume,1f,land2Variation);
        }
    }

    public void RocketStart(){
        if(mainTrack!=null && !rocketing) source6.timeSamples=mainTrack.timeSamples%source6.clip.samples;
        rocketing=true;
        falling=false;
    }

    public void RocketEnd(){
        rocketing=false;
    }

    public void Run(int speed){
        int prevRunningSpeed=runningSpeed;
        runningSpeed=speed;
        if(runningSpeed>0){
            if(runningSpeed!=prevRunningSpeed && mainTrack!=null){
                source4.clip=runningClips[runningSpeed-1];
                source4.Play();
                source4.timeSamples=mainTrack.timeSamples%source4.clip.samples;
                source7.Play();
                source7.timeSamples=mainTrack.timeSamples%source7.clip.samples;
                source8.Play();
                source8.timeSamples=mainTrack.timeSamples%source8.clip.samples;
            }
            running=true;
        }else{
            running=false;
        }
    }

    public void Skid(float speed){

    }

    public void Fall(){
        if(!rocketing){
            falling=true;
            if(!prevFalling && !source3.isPlaying){
                PlayRandomClip(fallingClips,2,fallingMaxVolume);
            }
        }
    }

    void PlayClip(AudioClip clip,int channel=0,float volume=1f,float pitch=1f){
        audioSources[channel].Stop();
        audioSources[channel].clip=clip;
        audioSources[channel].volume=volume*maxVolume;
        audioSources[channel].pitch=pitch;
        audioSources[channel].time=0f;
        audioSources[channel].Play();
    }

    void PlayRandomClip(AudioClip[] clips,int channel=0,float volume=1f,float pitch=1f,bool variation=false){
        AudioClip clip=clips[Random.Range(0,clips.Length)];
        if(variation){
            volume=volume-Random.Range(0,0.1f*maxVolume*volume);
            pitch=pitch+Random.Range(0,0.5f)-0.25f;
        }
        PlayClip(clip,channel,volume,pitch);
    }

    IEnumerator SyncTracks(){
        while(true){
            if(mainTrack!=null){
                Debug.Log("sync tracks");
                source6.timeSamples=mainTrack.timeSamples%source6.clip.samples;
                source4.timeSamples=mainTrack.timeSamples%source4.clip.samples;
                source7.timeSamples=mainTrack.timeSamples%source7.clip.samples;
                source8.timeSamples=mainTrack.timeSamples%source8.clip.samples;
            } 
            yield return new WaitForSeconds(0.5f);
        }
    }

}
