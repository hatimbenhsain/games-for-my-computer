using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class WordTracker : MonoBehaviour
{
    public string[] wordArray = new string[26];
    private int currentIndex = 0;

    public GameObject[] slots = new GameObject[26];

    private ComplimentManager complimentManager;

    private void Start(){
        complimentManager=FindObjectOfType<ComplimentManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Word"))
        {
            if (currentIndex < wordArray.Length)
            {
                wordArray[currentIndex] = other.gameObject.name;
                currentIndex++;

                MoveWordToSlot();
                //CheckArrayFull();

                //Adding Word to compliment array
                complimentManager.AddWord(other.gameObject.GetComponent<WordScript>().word);
            }
        }
    }
    
    private void MoveWordToSlot()
    {
        for (int i = 0; i < currentIndex; i++)
        {
            GameObject wordObject = GameObject.Find(wordArray[i]);
            if (wordObject != null && i < slots.Length)
            {
                wordObject.transform.position = slots[i].transform.position;
                wordObject.transform.rotation = Quaternion.identity; // Reset rotation to (0, 0, 0)


                Rigidbody wordRigidbody = wordObject.GetComponent<Rigidbody>();
                if (wordRigidbody != null)
                {
                    wordRigidbody.constraints = RigidbodyConstraints.FreezePositionX |
                                                RigidbodyConstraints.FreezePositionY |
                                                RigidbodyConstraints.FreezePositionZ;
                }
            }
        }
    }

    /*private void CheckArrayFull()
    {
        //TO DO: ideally this is resetting the level, but for now it destroys
        Destroy(GameObject.FindWithTag("Word"));
    }*/
}
