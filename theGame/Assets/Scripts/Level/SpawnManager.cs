using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject playerController;
    private ThirdPersonController thirdPersonController;
    public GameObject VFX;
    private PlayerVFX playerVFX;
    public float transformTime;
    public float vfxTime;
    void Start()
    {
        // name every spawnpoint like "SpawnPoint1"
        GameObject spawnPoint = GameObject.Find($"SpawnPoint{SpawnDataHolder.spawnLocationIndex}");


        if (spawnPoint != null)
        {
            playerPrefab.transform.position = spawnPoint.transform.position;
        }
        else
        {
            Debug.LogError("Spawn point not found.");
        }

        thirdPersonController = playerController.GetComponent<ThirdPersonController>();
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
        }
        //playerController.
        playerVFX = VFX.GetComponent<PlayerVFX>();
    }

    private IEnumerator StartRain(){
        yield return new WaitForSeconds(vfxTime+transformTime+0.1f);
        FindObjectOfType<RainThatTalks>().StartRain();
    }

    public int GetSpawnIndex(){
        return SpawnDataHolder.spawnLocationIndex;
    }

}
