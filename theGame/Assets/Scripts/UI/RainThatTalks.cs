using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Yarn.Unity;
using StarterAssets;

public class RainThatTalks : MonoBehaviour
{
    public Animator _animator;
    private Transform Player; // The target position and rotation for the player
    public ParticleSystem Rain;
    private ColorAdjustments colorAdjustments;
    public Color filterColor = Color.white; // Base target color
    private Color targetColor = Color.white; // Base target color
    private Color baseColor = Color.white; // Base color to lerp from
    private bool isLerping = false; // Flag to indicate if we're currently lerping
    public float lerpDuration = 2.0f; // Duration of the lerp in seconds
    private float lerpTimer = 0.0f; // Timer for the lerp

    private bool isRaining=false;


    // Start is called before the first frame update
    void Start()
    {
        Rain.Stop();
        GameObject playerCameraRootObject = GameObject.Find("PlayerCameraRoot");
        if (playerCameraRootObject != null)
        {
            Player = playerCameraRootObject.transform; 
        }
        else
        {
            Debug.LogError("PlayerCameraRoot not found in the scene.");
        }
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
            if (Input.GetKeyDown(KeyCode.R)) //PLACEHOLDER
            {
                StartRain();
            }
            if (Input.GetKeyDown(KeyCode.T)) //PLACEHOLDER
            {
                EndRain();
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

    [YarnCommand]
    public void StartRain(string rainNode=""){
        Debug.Log("start rain?");
        if(!isRaining){
            Debug.Log("start rain");
            _animator.SetTrigger("Rain");
            baseColor = colorAdjustments.colorFilter.value; // Update base color to current value
            targetColor = filterColor; 
            isLerping = true;
            lerpTimer = 0f; // Reset the timer
            Rain.Play();
            if(rainNode==""){       
                ThirdPersonController.CharacterState st=FindObjectOfType<ThirdPersonController>().currentState;
                rainNode="Rain1";
                switch(st){
                    case ThirdPersonController.CharacterState.Fish:
                        rainNode="Rain1";
                        break;
                    case ThirdPersonController.CharacterState.Leg:
                        rainNode="Rain2";
                        break;
                    case ThirdPersonController.CharacterState.Wing:
                        rainNode="Rain4";
                        break;
                    case ThirdPersonController.CharacterState.Rocket:
                        rainNode="Rain4";
                        break;
                }
            }
            FindObjectOfType<DialogueRunner>().StartDialogue(rainNode);
            isRaining=true;
        }
    }

    [YarnCommand]
    public void EndRain(){
        if(isRaining){
            _animator.SetTrigger("StopRain");
            baseColor = colorAdjustments.colorFilter.value; // Update base color to current value
            targetColor = Color.white; // Original color
            isLerping = true;
            lerpTimer = 0f; // Reset the timer
            Rain.Stop();
            isRaining=false;
        }
    }
}
