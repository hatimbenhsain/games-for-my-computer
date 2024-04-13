using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Package : MonoBehaviour
{
    public GameObject[] items;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("collision");
        //Open cardboard box and replace with random item
        if(other.gameObject.tag=="Player"){
            Instantiate(items[(int)Mathf.Floor(Random.Range(0,2.99f))],transform.position,Quaternion.identity);
            Destroy(transform.parent.parent.gameObject);
        }
    }
}
