using UnityEngine;
using Yarn.Unity;

public class SpriteChangerOnExit : MonoBehaviour
{
    // Assign this in the Unity Editor
    public Sprite newSprite;

    // Assign the specific GameObject you want to monitor (the object leaving the trigger)
    public GameObject targetObject;

    // The name of the Yarn node to trigger
    public string yarnNodeName;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private DialogueRunner dialogueRunner;

    void Start()
    {
        // Get the SpriteRenderer component attached to this NPC
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Get the AudioSource component attached to this NPC
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            // Initially disable the AudioSource if it shouldn't play immediately
            audioSource.enabled = false;
        }

        // Get the DialogueRunner component
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the collider is associated with the target object we are monitoring
        if (other.gameObject == targetObject)
        {
            if (spriteRenderer != null && newSprite != null)
            {
                spriteRenderer.sprite = newSprite;

                // Enable the AudioSource if it's not null
                if (audioSource != null)
                {
                    audioSource.enabled = true;

                    // Optionally, you can start playing if the AudioSource is set to not play on enable
                    audioSource.Play();
                }

                // Trigger the specified Yarn node
                if (dialogueRunner != null && !string.IsNullOrEmpty(yarnNodeName))
                {
                    dialogueRunner.StartDialogue(yarnNodeName);
                }
            }
        }
    }
}