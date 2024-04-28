using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyLevelProgress : MonoBehaviour
{
    public Image fillTop;
    public Image fillRight;
    public Image fillBottom;
    public Image fillLeft;
    
    public float fillSpeed = 0.5f; //rate that the progress bar will fill from 0-->1

    public AudioSource soundBite01;
    public AudioSource soundBite02;
    public AudioSource soundBite03;

    public GameObject Fever;

    public GameObject PartyLevel;
    

    // Update is called once per frame
    void Update()
    {
    fillingFeverBar();
    }

    void fillingFeverBar()
    {
        // when any key is pressed the progress bar's value will fill by the fill speed, otherwise if no key is pressed it slides back down
        if (Input.anyKey)
        {
            if (fillTop.fillAmount >= 0)
            {
                fillTop.fillAmount += fillSpeed * Time.deltaTime;
            }
            //fill bar 2 starts
            
            if (fillTop.fillAmount == 1)
            {
                fillRight.fillAmount += fillSpeed * Time.deltaTime;
            } 
            //fill bar 3 starts
            
            if (fillRight.fillAmount == 1)
            {
                fillBottom.fillAmount += fillSpeed * Time.deltaTime;
            }
            //final fill bar/left starts

            if (fillBottom.fillAmount == 1)
            {
                fillLeft.fillAmount += fillSpeed * Time.deltaTime;
            }

            if (fillLeft.fillAmount == 1)
            {
                PartyLevel.GetComponent<Animator>().SetBool("FeverOn", true);
                Fever.SetActive(true);
            }
        }

    }

}
