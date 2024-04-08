using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CharacterVFX : MonoBehaviour
{
    [SerializeField] VisualEffect complimentSmoke;
    public bool complimentVFX = false;
    // Start is called before the first frame update
    void Start()
    {
        complimentSmoke = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (complimentVFX)
        {
            complimentSmoke.Play();
            complimentVFX = false;
        }
    }

    public void TriggerComplimentVFX(){
        complimentVFX=true;
        Debug.Log("triggered");
    }
}
