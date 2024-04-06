using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class BossSphere : MonoBehaviour
{
    public float speed=2f;
    private float lifeLength=10f;
    private float lifeTime=0f;
    void Start()
    {
        transform.parent=transform.parent.parent.parent;
    }

    void Update()
    {
        Vector3 pos=transform.position;
        transform.position=pos+transform.forward*Time.deltaTime*speed;      
        lifeTime+=Time.deltaTime;
        if(lifeTime>lifeLength){
            Destroy(gameObject);
        } 
    }
}
