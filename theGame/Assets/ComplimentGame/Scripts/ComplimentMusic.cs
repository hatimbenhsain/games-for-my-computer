using UnityEngine;
using System.Collections;  // Ensure this namespace is included for IEnumerator

public class ComplimentMusic : MonoBehaviour
{
    public AudioClip startSound; // Assign this in the Unity Editor with your continuous sound.
    public GameObject targetPrefab; // Assign the target prefab for collision detection.
    public float fadeDuration = 2.0f; // Time in seconds to fade out the sound.

    private AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component attached to this GameObject.
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            Debug.LogWarning("LevelEnd script expects an AudioSource component on the same GameObject.");
            return;
        }

        // Start playing the sound.
        if (startSound)
        {
            audioSource.clip = startSound;
            audioSource.loop = true; // Loop the sound until it's faded out.
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the target prefab by comparing prefab instance IDs.
        if (other.gameObject == targetPrefab)
        {
            StartCoroutine(FadeOutSound(fadeDuration)); // Start fading out the sound.
        }
    }

    private IEnumerator FadeOutSound(float duration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Reset the volume if you need to reuse this audio source later.
    }
}