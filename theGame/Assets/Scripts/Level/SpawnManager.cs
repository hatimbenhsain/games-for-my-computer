using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject playerController;
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

        var thirdPersonController = playerController.GetComponent<ThirdPersonController>();
        if (thirdPersonController != null)
        {
            Debug.Log(SpawnDataHolder.characterState);
            thirdPersonController.currentState = SpawnDataHolder.characterState;
        }

    }
}
