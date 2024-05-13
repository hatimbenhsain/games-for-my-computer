using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSquash : MonoBehaviour
{
    public float frequency = 1.0f; 
    public float amplitude = 0.5f; 
    private Vector3 originalScale; 

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // calculate the new scale factors based on a sine wave
        float scale = Mathf.Sin(Time.time * frequency) * amplitude + 1.0f;

        transform.localScale = new Vector3(originalScale.x * scale, originalScale.y / scale, originalScale.z);
    }
}
