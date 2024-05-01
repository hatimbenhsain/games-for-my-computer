using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumGameManager : MonoBehaviour
{
    public bool isFloating = true;
    GameObject floatingObjects;
    void Start()
    {
        floatingObjects = GameObject.Find("FloatingObjects");
        Cursor.visible = false;
    }
    
    void Update()
    {
        if (!isFloating)
        {
            DisableAllFloating(floatingObjects);
        }
    }

    // make all the floating object not floating
    private void DisableAllFloating(GameObject parent)
    {
        FloatingObjects[] floatingObjects = parent.GetComponentsInChildren<FloatingObjects>();
        foreach (FloatingObjects floater in floatingObjects)
        {
            floater.enableFloating = false;
        }
    }
}
