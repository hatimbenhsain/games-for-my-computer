using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCollider : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag=="Player"){
            Debug.Log("is player");
            FindObjectOfType<GameManager>().Respawn();
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag=="Player"){
            Debug.Log("is player");
            FindObjectOfType<GameManager>().Respawn();
        }
    }
}
