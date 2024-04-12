using System.Collections;
using UnityEngine;

public class UIBounceEffect : MonoBehaviour
{
    public float scaleFactor = 1.1f; // The scale factor to which the UI element will grow
    public float duration = 0.5f; // Duration of one half of the animation

    private Vector3 originalScale; // To store the original scale
    private AudioSource audioSource; // Audio source component

    void Awake()
    {
        originalScale = transform.localScale;
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        // Start the bounce animation
        StartCoroutine(BounceEffect());

        // Play the sound
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    IEnumerator BounceEffect()
    {
        // Scale up
        yield return StartCoroutine(Scale(originalScale * scaleFactor, duration));
        // Scale down
        yield return StartCoroutine(Scale(originalScale, duration));
    }

    IEnumerator Scale(Vector3 targetScale, float duration)
    {
        Vector3 currentScale = transform.localScale;
        float counter = 0;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            transform.localScale = Vector3.Lerp(currentScale, targetScale, counter / duration);
            yield return null;
        }
    }
}
