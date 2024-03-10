using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyLevelProgress : MonoBehaviour
{
    public Slider progress;
    
    public float fillSpeed = 0.5f; //rate that the progress bar will fill from 0-->1

    public AudioSource soundBite01;
    public AudioSource soundBite02;
    public AudioSource soundBite03;
    

    // Update is called once per frame
    void Update()
    {
        // when any key is pressed the progress bar's value will fill by the fill speed, otherwise if no key is pressed it slides back down
        if (Input.anyKey)
        {
            progress.value += fillSpeed * Time.deltaTime;
        }
        else
        {
            progress.value -= fillSpeed * Time.deltaTime;
        }
        
        //could be fixed, kind of janky. If the progress value is between 0 & 0.5 "Seriously?" plays
        if (progress.value >= 0 && progress.value <= 0.5f)
        {
            soundBite01.Play();
        }

        //If the progress value between 0.6 & o.9 is achieved, play "clapping hands wooooo"
        if (progress.value >= 0.6f && progress.value <= 0.9f)
        {
            soundBite03.Play();
        }
        //If progress bar reaches 1 play "wow shes really good!"
        if (progress.value == 1)
        {
            soundBite02.Play();
        }
    }


}
