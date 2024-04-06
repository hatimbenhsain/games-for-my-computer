using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;//need this to be able to use yarn command

public class VoiceSwitch : MonoBehaviour
{
    public AudioSource voiceAudioSource;

    public AudioClip playerClip;
    public AudioClip npcClip;

   // private AudioClip currentClip;

    // Start is called before the first frame update
    void Start()
    {
       //currentClip = voiceAudioSource.GetComponent<AudioSource>().clip;
        
    }

    [YarnCommand]//hey yarn pay attention to this cause its a command
    public void ChangeSoundtoCharacter()//name of the command 
    {
        voiceAudioSource.clip = npcClip;
        voiceAudioSource.Play();

    }

        [YarnCommand]//hey yarn pay attention to this cause its a command
    public void ChangeSoundtoPlayer()//name of the command 
    {
        voiceAudioSource.clip = playerClip;
        voiceAudioSource.Play();
    }

}
