using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class RainThatTalks : MonoBehaviour
{
    public Animator _animator;
    public Transform Player; // The target position and rotation for the player
    public ParticleSystem Rain;
    private ColorAdjustments colorAdjustments;
    private Color targetColor = Color.white; // Base target color
    private Color baseColor = Color.white; // Base color to lerp from
    private bool isLerping = false; // Flag to indicate if we're currently lerping
    public float lerpDuration = 2.0f; // Duration of the lerp in seconds
    private float lerpTimer = 0.0f; // Timer for the lerp

    // Start is called before the first frame update
    void Start()
    {
        Rain.Stop();
        // Find the Global Volume GameObject by name
        GameObject globalVolumeObject = GameObject.Find("Global Volume");
        if (globalVolumeObject)
        {
            Volume globalVolume = globalVolumeObject.GetComponent<Volume>();
            if (globalVolume && globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
            {
                baseColor = colorAdjustments.colorFilter.value;
            }
            else
            {
                Debug.LogError("Color Adjustments not found on the Global Volume.");
            }
        }
        else
        {
            Debug.LogError("Global Volume not found in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Rain.transform.position = Player.transform.position;
        if ( _animator != null )
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _animator.SetTrigger("Rain");
                baseColor = colorAdjustments.colorFilter.value; // Update base color to current value
                targetColor = new Color(0.45f, 0.45f, 0.5f); // New target color
                isLerping = true;
                lerpTimer = 0f; // Reset the timer
                Rain.Play();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                _animator.SetTrigger("StopRain");
                baseColor = colorAdjustments.colorFilter.value; // Update base color to current value
                targetColor = Color.white; // Original color
                isLerping = true;
                lerpTimer = 0f; // Reset the timer
                Rain.Stop();
            }
        }
        if (isLerping)
        {
            // Increment the timer by the fraction of time that has passed
            lerpTimer += Time.deltaTime;
            float lerpProgress = lerpTimer / lerpDuration;

            // Lerp the color filter value
            colorAdjustments.colorFilter.value = Color.Lerp(baseColor, targetColor, lerpProgress);

            // Check if the lerp is complete
            if (lerpProgress >= 1.0f)
            {
                isLerping = false;
            }
        }
    }
}
