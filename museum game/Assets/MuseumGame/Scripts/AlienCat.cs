using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienCat : MonoBehaviour
{
    public GameObject catPocket;
    public GameObject alienCatRetrieved;

    private void OnTriggerExit(Collider other)
    {
        catPocket.SetActive(true);
        alienCatRetrieved.SetActive(true);
        gameObject.SetActive(false);
    }
}

