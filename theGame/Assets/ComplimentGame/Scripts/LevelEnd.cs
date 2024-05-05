using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public AudioClip startSound; // Assign this in the Unity Editor with your continuous sound.
    public AudioClip collisionSound; // Assign this in the Unity Editor with your sound effect.
    public AudioClip otherSound; // Assign this in the Unity Editor, the other sound to manage.

    private AudioSource audioSource;
    private AudioSource otherAudioSource; // This could potentially be the same as audioSource if only one source is used.

    private void Start()
    {
        // Get the AudioSource component attached to this GameObject.
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            Debug.LogWarning("LevelEnd script expects an AudioSource component on the same GameObject.");
            return;
        }

        // You can choose to use the same AudioSource or find another one in your scene to play other sounds.
        otherAudioSource = audioSource; // If using the same source, or find another source as needed.

        // Stop the other sound if it's currently playing.
        if (otherSound && otherAudioSource.isPlaying && otherAudioSource.clip == otherSound)
        {
            otherAudioSource.Stop();
        }

        // Start playing the continuous sound if assigned.
        if (startSound)
        {
            audioSource.clip = startSound;
            audioSource.loop = true;  // Loop the sound until it's stopped.
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ComplimentSphere")
        {
            BallSnapper ballSnapper = other.gameObject.GetComponent<BallSnapper>();
            if (ballSnapper != null)
            {
                ballSnapper.enabled = false;
            }

            Collider ballCollider = other.gameObject.GetComponent<Collider>();
            if (ballCollider != null)
            {
                ballCollider.enabled = false;
            }

            if (audioSource && collisionSound)
            {
                audioSource.Stop(); // Stop the continuous sound first.
                audioSource.PlayOneShot(collisionSound);  // Play the collision sound effect.
            }

            FindObjectOfType<ComplimentManager>().ComplimentEnd();

            // Restart the other sound if needed.
            if (otherSound)
            {
                otherAudioSource.clip = otherSound;
                otherAudioSource.Play();  // Restart the other sound.
            }
        }
    }
}
