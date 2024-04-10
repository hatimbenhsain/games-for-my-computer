using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public int songIndex = 0; // Assign this in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MusicManager musicManager = FindObjectOfType<MusicManager>(); // Find the MusicManager in the scene
            if (musicManager != null)
            {
                musicManager.ChangeSong(songIndex);
            }
        }
    }
}