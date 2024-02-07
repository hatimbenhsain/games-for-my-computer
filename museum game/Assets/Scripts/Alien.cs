using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public void Interact()
    {
        // now the aliens just disappear
        gameObject.SetActive(false);
    }
}
