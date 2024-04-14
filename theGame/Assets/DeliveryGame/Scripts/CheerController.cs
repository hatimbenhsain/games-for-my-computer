using UnityEngine;
using UnityEngine.UI;

public class CheerController : MonoBehaviour
{
    // Array of AudioClips to cycle through
    public AudioClip[] soundClips;
    private int currentClipIndex = 0;

    // Reference to the button in the UI
    public Button yourButton;

    void Start()
    {
        // Add a listener to the button to call the PlaySound method when clicked
        yourButton.onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        // Create a new GameObject with an AudioSource to play the sound
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = soundClips[currentClipIndex];
        audioSource.Play();

        // Destroy the GameObject after the clip finishes playing
        Destroy(soundGameObject, audioSource.clip.length);

        // Move to the next clip in the array, looping back to the first if necessary
        currentClipIndex = (currentClipIndex + 1) % soundClips.Length;
    }
}