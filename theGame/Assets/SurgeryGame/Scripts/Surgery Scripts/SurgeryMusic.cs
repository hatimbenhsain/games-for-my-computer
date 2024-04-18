using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SurgeryMusic : MonoBehaviour
{
    public AudioClip[] songs;
    public List<AudioSource> audioSources;
    public GameObject songPlayer;

    public float maxVolume=0.75f;

    public float fadeTime=1f;
    private float fadeTimer=0f;
    private bool fading=false;

    public int songToFadeFrom;
    public int songToFadeTo=0;
    // Start is called before the first frame update
    void Start()
    {
        audioSources=new List<AudioSource>();
        foreach(AudioClip clip in songs){
            GameObject sp=Instantiate(songPlayer,transform);
            sp.GetComponent<AudioSource>().clip=clip;
            audioSources.Add(sp.GetComponent<AudioSource>());
        }
        songToFadeFrom=-1;
        audioSources[songToFadeTo].Play();
        audioSources[songToFadeTo].volume=1;
    }

    // Update is called once per frame
    void Update()
    {
        if(fading){
            fadeTimer+=Time.deltaTime;
            fadeTimer=Mathf.Clamp(fadeTimer,0,fadeTime);
            audioSources[songToFadeTo].volume=maxVolume*fadeTimer/fadeTime;
            if(songToFadeFrom>=0){
                audioSources[songToFadeFrom].volume=maxVolume*(1-fadeTimer/fadeTime);
            }
            if(fadeTimer>=fadeTime){
                fading=false;
                if(songToFadeFrom>=0){
                    audioSources[songToFadeFrom].Pause();
                }
            }
        }
    }

    void FadeIn(){

    }

    void FadeOut(){

    }

    public void CrossFade(int i){
        if(songToFadeFrom>=0){
            audioSources[songToFadeFrom].Pause();
            audioSources[songToFadeFrom].volume=0;
        }
        fading=true;
        songToFadeFrom=songToFadeTo;
        songToFadeTo=i;
        audioSources[songToFadeTo].volume=0;
        audioSources[songToFadeTo].Play();
        fadeTimer=0f;
    }

}
