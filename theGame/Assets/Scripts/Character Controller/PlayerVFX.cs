using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFX: MonoBehaviour
{
    private Vector3 cameraDir;
    private Transform followPlayer; // The target position and rotation for the player
    private Transform smokeTransform;
    private ThirdPersonController thirdPersonController;
    [SerializeField] GameObject PlayerController;
    // VFX
    [SerializeField] VisualEffect SquirtFront;
    [SerializeField] VisualEffect Smoke;
    [SerializeField] VisualEffect landSmoke;
    [SerializeField] VisualEffect magicSmoke;
    private bool inAir;
    public bool playMCMagicSmoke = false;

    // Start is called before the first frame update

    void Start()
    {
        Smoke.enabled = false;
        landSmoke.enabled = false;
        SquirtFront.enabled = false;
        SquirtFront.enabled = false;
        //magicSmoke.enabled = false;
        //StartCoroutine(EnableVFXWithDelay(3f));
        thirdPersonController = PlayerController.GetComponent<ThirdPersonController>(); // Get the third person controller script
        GameObject playerCameraRootObject = GameObject.Find("PlayerController"); // Get the player transform
        if (playerCameraRootObject != null)
        {
            followPlayer = playerCameraRootObject.transform;
        }
        else
        {
            Debug.LogError("PlayerController not found in the scene.");
        }
        GameObject smoke = GameObject.Find("Smoke"); // Get the player transform
        if (playerCameraRootObject != null)
        {
            smokeTransform = smoke.transform;
        }
        else
        {
            Debug.LogError("PlayerController not found in the scene.");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        cameraDir = Camera.main.transform.forward;
        cameraDir.y = 0;

        transform.rotation = Quaternion.LookRotation(cameraDir); // set rotation based on camera
        if (playMCMagicSmoke)
        {
            transform.position = followPlayer.position; // set transform to the player transform
            magicSmoke.Play();
            playMCMagicSmoke = false;
        }
        if (!thirdPersonController.Grounded)
        {
            inAir = true;
        }
        if (inAir && thirdPersonController.Grounded) 
        {
            inAir = false;
            transform.position = followPlayer.position; // set transform to the player transform
            if (thirdPersonController.currentState == ThirdPersonController.CharacterState.Fish || thirdPersonController.IsCrouching())
            {
                SquirtFront.enabled = true;
                //Debug.Log("Squirt");
                SquirtFront.Play();
                // play squirt when landing on the floor in fish
            }
            else
            {
                landSmoke.SetVector2("SetSizeRandom", new Vector2((0.2f - 0.02f * thirdPersonController.playerFallSpeed), (0.3f - 0.05f * thirdPersonController.playerFallSpeed)));
                landSmoke.SetVector3("VelocityMax", new Vector3((1f - 0.2f * thirdPersonController.playerFallSpeed), (0.5f - 0.05f * thirdPersonController.playerFallSpeed), (1f - 0.2f * thirdPersonController.playerFallSpeed)));
                landSmoke.SetVector3("VelocityMin", new Vector3((-1f + 0.2f * thirdPersonController.playerFallSpeed), 0.1f, (-1f + 0.2f * thirdPersonController.playerFallSpeed)));
                landSmoke.enabled = true;
                Debug.Log("Jump");
                landSmoke.Play();
                // plan land smoke when landing on the floor in other mode
            }

        }
        if (thirdPersonController.isSkidding && thirdPersonController.currentState != ThirdPersonController.CharacterState.Fish)
        {
            Smoke.enabled = true;
            Smoke.SetFloat("Rate", (14f + 0.5f * thirdPersonController.playerSpeed));
            Smoke.SetVector2("SetSizeRandom", new Vector2((0.1f + 0.07f * thirdPersonController.playerSpeed), (0.15f + 0.11f * thirdPersonController.playerSpeed)));
            Smoke.SetBool("Loop", true);
            if (thirdPersonController.flipped)
            {
                smokeTransform.position = followPlayer.position - 0.35f * Camera.main.transform.right;
                smokeTransform.eulerAngles = transform.eulerAngles + new Vector3 (0f, 0f, 0f);
            }
            else
            {
                smokeTransform.position = followPlayer.position + 0.35f * Camera.main.transform.right;
                smokeTransform.eulerAngles = transform.eulerAngles + new Vector3(0f, 180f, 0f);
            }
            Smoke.enabled = true;
        }
        else
        {
            Smoke.SetBool("Loop", false);
        }
        
    }

    IEnumerator EnableVFXWithDelay(float delay)
    {
        Debug.Log("enabled");
        yield return new WaitForSeconds(delay); 

    }
}
