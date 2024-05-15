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

    public Sprite microbeAlienImage;
    private MuseumGameManager museumGameManager;

    private DialogueRunner dialogueRunner;
    public float interactionRadius=3f;

    private FirstPersonController fpc;
    private void Start()
    {
        // Get a reference to the first-person camera.
        playerCamera = GetComponentInChildren<Camera>();
        dialogueRunner=FindObjectOfType<DialogueRunner>();
        museumGameManager = FindObjectOfType<MuseumGameManager>();
        fpc=FindObjectOfType<FirstPersonController>();
    }

    private void Update()
    {
        // Perform a ray cast from the camera's position and direction.
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayer))
        {
            // Check if the hit object has a script or component that allows interaction.
            Alien alien = hit.collider.GetComponent<Alien>();
            NPC npc = hit.collider.GetComponent<NPC>(); // Also checking for NPC component at the same point
            if (alien != null)
            {
                if (alien.isNormalArtwork == false)
                {
                    museumGameManager.isAlienScanned = true;
                }
            }

            museumGameManager.isAlien = true;
            // new dialogue interaction only 

            // Check for player input to interact.
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (alien != null)
                {
                    // Call the interaction function on the object.
                    //alien.Interact();
                }
            }

            if (Input.GetKeyUp(KeyCode.E) && !dialogueRunner.IsDialogueRunning && museumGameManager.isAlien && npc != null && !string.IsNullOrEmpty(npc.talkToNode))
            {
                //CheckForNearbyNPC();
                dialogueRunner.StartDialogue(npc.talkToNode);
            }
        }
        else
        {
            museumGameManager.isAlien = false;
            museumGameManager.isAlienScanned = false;
        }
       


        if(dialogueRunner.IsDialogueRunning){
            fpc.playerCanMove=false;
            fpc.enableHeadBob=false;
        }else{
            fpc.playerCanMove=true;
            fpc.enableHeadBob=true;
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
