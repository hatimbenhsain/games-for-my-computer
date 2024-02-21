using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRotation : MonoBehaviour
{
    public Color[] fogColors; // Array of fog colors to rotate through
    public Color[] ambientColors; // Array of ambient colors to rotate through
    public float rotationSpeed = 1f; // Speed of color rotation
    public float lerpSpeed = 0.5f; // Speed of color transition

    private int fogIndex = 0;
    private int ambientIndex = 0;
    private Color fogTargetColor;
    private Color ambientTargetColor;

    void Start()
    {
        fogTargetColor = fogColors[fogIndex];
        ambientTargetColor = ambientColors[ambientIndex];
    }

    void Update()
    {
        // Rotate between colors for fog
        float fogT = Mathf.PingPong(Time.time * rotationSpeed, fogColors.Length - 1);
        fogIndex = Mathf.FloorToInt(fogT);
        fogTargetColor = fogColors[fogIndex];

        // Rotate between colors for ambient
        float ambientT = Mathf.PingPong(Time.time * rotationSpeed, ambientColors.Length - 1);
        ambientIndex = Mathf.FloorToInt(ambientT);
        ambientTargetColor = ambientColors[ambientIndex];

        // Gradually transition fog color
        RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, fogTargetColor, lerpSpeed * Time.deltaTime);

        // Gradually transition ambient color
        RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, ambientTargetColor, lerpSpeed * Time.deltaTime);
    }
}
