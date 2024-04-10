using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject playerController;
    public ThirdPersonController thirdPersonController;
    public GameObject VFX;
    private PlayerVFX playerVFX;
    public float moveTime = 0.1f;
    public float transformTime;
    public float vfxTime;
    void Start()
    {
        playerVFX = GameObject.Find("PlayerVFX")?.GetComponent<PlayerVFX>();
        GameObject spawnPoint = GameObject.Find($"SpawnPoint{SpawnDataHolder.spawnLocationIndex}");
        Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        thirdPersonController = GameObject.Find("PlayerController")?.GetComponent<ThirdPersonController>();
        // name every spawnpoint like "SpawnPoint1"

        Debug.Log(SpawnDataHolder.spawnLocationIndex);

            //playerPrefab.transform.position = spawnPoint.transform.position;
            //playerPrefab.transform.rotation = spawnPoint.transform.rotation;

        //thirdPersonController = playerController.GetComponent<ThirdPersonController>();
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
        //playerVFX = VFX.GetComponent<PlayerVFX>();
    }

    private IEnumerator StartRain(){
        yield return new WaitForSeconds(vfxTime+transformTime+0.1f);
        FindObjectOfType<RainThatTalks>().StartRain();
    }

}
