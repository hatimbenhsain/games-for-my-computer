using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class turnOffRadio : MonoBehaviour
{
    public AudioSource radio;

    private void OnTriggerExit(Collider other)
    {
        radio.Stop();
        Destroy(gameObject);
    }
}
