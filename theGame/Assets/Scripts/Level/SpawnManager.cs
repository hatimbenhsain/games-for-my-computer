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
            thirdPersonController.currentState = SpawnDataHolder.characterState;
        }
        // if playTransformAnimation is set to true
        if (SpawnDataHolder.playTransformAnimation)
        {
            StartCoroutine(PlayerTransformCoroutine());
            // lock movement
            thirdPersonController.inTransform = true;
        }
        //playerController.
        playerVFX = VFX.GetComponent<PlayerVFX>();
    }

    private IEnumerator PlayerTransformCoroutine()
    {
        // wait
        yield return new WaitForSeconds(vfxTime);
        // play smoke
        playerVFX.PlayMagicSmoke();
        // wait until the smoke happen
        yield return new WaitForSeconds(transformTime);
        // change state
        thirdPersonController.SetState(SpawnDataHolder.targetCharacterState);
        // allow movement
        thirdPersonController.inTransform = false;
        FindObjectOfType<RainThatTalks>().StartRain();
    }

    public int GetSpawnIndex(){
        return SpawnDataHolder.spawnLocationIndex;
    }

}
