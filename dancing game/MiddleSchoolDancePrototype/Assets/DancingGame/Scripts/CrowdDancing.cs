using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdDancing : MonoBehaviour
{
    public float danceSpeed = 1f; // Adjust this to control the speed of the dance
    public float danceMagnitude = 0.1f; // Adjust this to control the magnitude of the dance

    private Vector3 originalPosition;
    private float randomOffsetX;
    private float randomOffsetY;

    void Start()
    {
        originalPosition = transform.position;
        // Generate random offsets for each instance
        randomOffsetX = Random.Range(0f, 1f);
        randomOffsetY = Random.Range(0f, 1f);
    }

    void Update()
    {
        // Calculate new position based on sine wave oscillations with random offsets
        float danceOffsetX = Mathf.Sin((Time.time + randomOffsetX) * danceSpeed) * danceMagnitude;
        float danceOffsetY = Mathf.Sin((Time.time + randomOffsetY) * danceSpeed) * danceMagnitude;
        Vector3 newPos = originalPosition + new Vector3(danceOffsetX, danceOffsetY, 0f);

        // Move the object
        transform.position = newPos;
    }
}
