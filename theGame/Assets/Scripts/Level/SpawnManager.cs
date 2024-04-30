using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject playerController;
    private ThirdPersonController thirdPersonController;
    private GameObject VFX;
    private PlayerVFX playerVFX;
    public float moveTime = 0.1f;
    public float transformTime;
    public float vfxTime;
    private bool danceFinished = false;
    private bool surgeryFinished = false;
    private bool complimentFinished = false;
    private bool museumFinished = false;

    public GameObject school;
    public GameObject hospital;
    public GameObject museum;

    void Start()
    {
        playerVFX = GameObject.Find("PlayerVFX")?.GetComponent<PlayerVFX>();
        GameObject spawnPoint = GameObject.Find($"SpawnPoint{SpawnDataHolder.spawnLocationIndex}");
        Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        thirdPersonController = GameObject.Find("PlayerController")?.GetComponent<ThirdPersonController>();
        // name every spawnpoint like "SpawnPoint1"

        Debug.Log(SpawnDataHolder.spawnLocationIndex);
        if (thirdPersonController != null)
        {
            Debug.Log(SpawnDataHolder.characterState);
            thirdPersonController.SetState(SpawnDataHolder.characterState);
        }
        // if playTransformAnimation is set to true
        if (SpawnDataHolder.playTransformAnimation)
        {
            thirdPersonController.Metamorphosis(SpawnDataHolder.targetCharacterState,vfxTime,transformTime);
            StartCoroutine("StartRain");
        }else if(SpawnDataHolder.playRain){
            StartCoroutine("StartRain");
        }
        switch (SpawnDataHolder.spawnLocationIndex)
        {
            case 1:
                danceFinished = true;
                break;
            case 2:
                danceFinished = true;
                surgeryFinished = true;
                break;
            case 3:
                danceFinished = true;
                surgeryFinished = true;
                complimentFinished = true;
                break;
            case 4:
                danceFinished = true;
                surgeryFinished = true;
                complimentFinished = true;
                museumFinished = true;
                break;

        }
        if (danceFinished)
        {
            Collider schoolCollider = school.GetComponent<Collider>();
            if (schoolCollider != null)
            {
                schoolCollider.isTrigger = !schoolCollider.isTrigger;
            }

            GameObject schoolLevelLoader = GameObject.Find("LevelLoaderSchool");
            if (schoolLevelLoader != null)
            {
                schoolLevelLoader.gameObject.SetActive(false);
            }
        }
        if (surgeryFinished)
        {
            Collider hospitalCollider = hospital.GetComponent<Collider>();
            if (hospitalCollider != null)
            {
                hospitalCollider.isTrigger = !hospitalCollider.isTrigger;
            }

            GameObject hospitalLevelLoader = GameObject.Find("LevelLoaderHospital");
            if (hospitalLevelLoader != null)
            {
                hospitalLevelLoader.gameObject.SetActive(false);
            }
        }
        if (museumFinished)
        {
            Collider museumCollider = museum.GetComponent<Collider>();
            if (museumCollider != null)
            {
                museumCollider.isTrigger = !museumCollider.isTrigger;
            }

            GameObject museumlLevelLoader = GameObject.Find("LevelLoaderMuseum");
            if (museumlLevelLoader != null)
            {
                museumlLevelLoader.gameObject.SetActive(false);
            }
        }
        if (complimentFinished)
        {
            // TODO: toggle off compliment games
        }
    }

    private IEnumerator StartRain(){
        yield return new WaitForSeconds(vfxTime+transformTime+0.1f);
        FindObjectOfType<RainThatTalks>().StartRain();
    }

}
