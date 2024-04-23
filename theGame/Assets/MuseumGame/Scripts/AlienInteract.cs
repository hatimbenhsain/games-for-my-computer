using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Yarn.Unity;
using Yarn.Unity.Example;


public class AlienInteract : MonoBehaviour
{
    public float interactionDistance = 3f; //Adjust the interaction distance as needed.
    public LayerMask interactableLayer;    //Define a layer for objects that can be interacted with.

    private Camera playerCamera;

    public GameObject microbeDetector;
    public Sprite microbeImage;
    public Sprite microbeAlienImage;

    private bool microbeOn = false;

    private DialogueRunner dialogueRunner;
    public float interactionRadius=3f;
    private void Start()
    {
        // Get a reference to the first-person camera.
        playerCamera = GetComponentInChildren<Camera>();
        microbeDetector.GetComponent<Image>().sprite = microbeImage;
        dialogueRunner=FindObjectOfType<DialogueRunner>();
    }

    private void Update()
    {
        if (microbeOn)
        {
            microbeDetector.GetComponent<Image>().sprite = microbeAlienImage;
        }
        else
        {
            microbeDetector.GetComponent<Image>().sprite = microbeImage;
        }
        // Perform a ray cast from the camera's position and direction.
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayer))
        {
            // Check if the hit object has a script or component that allows interaction.
            Alien alien = hit.collider.GetComponent<Alien>();
            microbeOn = true;
            
            // Check for player input to interact.
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (alien != null)
                {
                    // Call the interaction function on the object.
                    //alien.Interact();
                }
            }
        }
        else
        {
            microbeOn = false;
        }
        
        microbeDetector.SetActive(false);
        
        if (Input.GetMouseButton(1))
        {
            microbeDetector.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.E) && !dialogueRunner.IsDialogueRunning){
            CheckForNearbyNPC();
        }

    }

    public void CheckForNearbyNPC()
    {
        List<NPC> allParticipants = new List<NPC>(FindObjectsOfType<NPC>());
        var target = allParticipants.Find(delegate (NPC p)
        {
            return string.IsNullOrEmpty(p.talkToNode) == false && // has a conversation node?
            (p.transform.position - this.transform.position)// is in range?
            .magnitude <= interactionRadius;
        });
        if (target != null)
        {
            // Kick off the dialogue at this node.
            FindObjectOfType<DialogueRunner>().StartDialogue(target.talkToNode);
            //npcTalkingTo=target;
            // reenabling the input on the dialogue
            //dialogueInput.enabled = true;
        }
    }
}
