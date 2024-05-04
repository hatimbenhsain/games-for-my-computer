using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is for setting the location of where the respawning will happen
public class RespawnLocationTrigger : MonoBehaviour
{

    //Where does the respawn happen?
    public Transform respawnTransform;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag=="Player"){
            FindObjectOfType<GameManager>().SetRespawnTransform(respawnTransform);
        }
    }
}
