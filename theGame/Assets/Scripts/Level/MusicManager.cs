using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicSource; // The AudioSource component for playing music
    public List<AudioClip> songs = new List<AudioClip>(); // List of songs to play
    public float fadeTime = 2f; // Duration for the fade effect
    private int currentSongIndex = -1; // Keeps track of the currently playing song

    void Start()
    {
        // Optionally, start playing the initial song here
        if (songs.Count > 0)
        {
            ChangeSong(0); // Start with the first song
        }
    }

    public void ChangeSong(int newSongIndex)
    {
        if (newSongIndex != currentSongIndex && newSongIndex < songs.Count)
        {
            StartCoroutine(FadeChangeSong(newSongIndex));
        }
    }

    private IEnumerator FadeChangeSong(int newSongIndex)
    {
        // Fade out the current song if it's playing
        if (currentSongIndex != -1)
        {
            for (float t = 0; t < fadeTime; t += Time.deltaTime)
            {
                musicSource.volume = (1 - t / fadeTime);
                yield return null;
            }
            musicSource.Stop();
            musicSource.volume = 1; // Reset volume
        }

        // Update the current song index
        currentSongIndex = newSongIndex;

        // Set the new song and fade it in
        musicSource.clip = songs[newSongIndex];
        musicSource.Play();

        musicSource.volume = 0; // Start at 0 volume to fade in
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            musicSource.volume = t / fadeTime;
            yield return null;
        }

        musicSource.volume = 1; // Ensure volume is back to full after fade in
    }
}