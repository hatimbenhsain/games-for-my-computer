using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObjects : MonoBehaviour
{
    public bool enableFloating = true;
    public float amplitude = 0.5f; 
    public float frequency = 1f; 

    private Vector3 startPos;
    private Rigidbody rb;
    private float randomAmplitudeOffset;
    private float randomFrequencyOffset;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        // apply random factors to amplitude and frequency
        randomAmplitudeOffset = Random.Range(-0.2f, 0.2f);
        randomFrequencyOffset = Random.Range(-0.2f, 0.2f);

        ConfigureRigidbody(enableFloating);
    }

    void Update()
    {
        // set the objects to floating
        if (enableFloating)
        {
            if (!rb.isKinematic)
            {
                ConfigureRigidbody(true);
            }
            Float();
        }
        else
        {
            if (rb.isKinematic)
            {
                ConfigureRigidbody(false);
            }
        }
    }

    void Float()
    {
        // make the floating objects move vertically in a sin wave
        Vector3 newPos = startPos;
        float currentAmplitude = amplitude + randomAmplitudeOffset;
        float currentFrequency = frequency + randomFrequencyOffset;
        newPos.y += Mathf.Sin(Time.time * Mathf.PI * currentFrequency) * currentAmplitude;

        if (rb != null)
        {
            rb.MovePosition(newPos);
        }
        else
        {
            transform.position = newPos;
        }
    }

    void ConfigureRigidbody(bool isFloating)
    {
        // set the rigid body to true if not floating and vice versa
        if (rb != null)
        {
            rb.isKinematic = isFloating;
            rb.useGravity = !isFloating;
        }
    }
}
