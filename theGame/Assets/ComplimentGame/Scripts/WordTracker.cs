using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordTracker : MonoBehaviour
{
    public string[] wordArray = new string[3];
    private int currentIndex = 0;

    public GameObject[] slots = new GameObject[3];

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
