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
    
    
    public float maxGrowthSpeed=0.5f;
    public float growthSpeedIncrement=0.1f;
    public float growthDeceleration=0.5f;
    public float minGrowthSpeed=-0.5f;

    public float currentGrowthSpeed;
    public float feverAmount;

    public GameObject fish;

    // Update is called once per frame
    private void Start() {
        feverAmount=0f;
        currentGrowthSpeed=0f;
    }

    void Update()
    {
        fillingFeverBar();
        
    }

    void fillingFeverBar()
    {
        currentGrowthSpeed-=growthDeceleration*Time.deltaTime;

        // when any key is pressed the progress bar's value will fill by the fill speed, otherwise if no key is pressed it slides back down
        if (!fish.activeInHierarchy && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)
        || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))){
            currentGrowthSpeed+=growthSpeedIncrement; 
        }

        currentGrowthSpeed=Mathf.Clamp(currentGrowthSpeed,minGrowthSpeed,maxGrowthSpeed);

        feverAmount+=currentGrowthSpeed*Time.deltaTime;
        feverAmount=Mathf.Clamp(feverAmount,0,4);

        fillTop.fillAmount=Mathf.Clamp(feverAmount,0,1);
        fillRight.fillAmount=Mathf.Clamp(feverAmount-1,0,1);
        fillBottom.fillAmount=Mathf.Clamp(feverAmount-2,0,1);
        fillLeft.fillAmount=Mathf.Clamp(feverAmount-3,0,1);

            //         if (fillTop.fillAmount >= 0)
            // {
            //     fillTop.fillAmount += fillSpeed * Time.deltaTime;
            // }
            // //fill bar 2 starts
            
            // if (fillTop.fillAmount == 1)
            // {
            //     fillRight.fillAmount += fillSpeed * Time.deltaTime;
            // } 
            // //fill bar 3 starts
            
            // if (fillRight.fillAmount == 1)
            // {
            //     fillBottom.fillAmount += fillSpeed * Time.deltaTime;
            // }
            // //final fill bar/left starts

            // if (fillBottom.fillAmount == 1)
            // {
            //     fillLeft.fillAmount += fillSpeed * Time.deltaTime;
            // }

        if (fillLeft.fillAmount >= 0.95)
        {
            PartyLevel.GetComponent<Animator>().SetBool("FeverOn", true);
            Fever.SetActive(true);
        }else{
            PartyLevel.GetComponent<Animator>().SetBool("FeverOn", false);
            Fever.SetActive(false);
        }

    }

}
