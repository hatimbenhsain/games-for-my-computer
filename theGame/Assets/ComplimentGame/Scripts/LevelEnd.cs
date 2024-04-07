using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public GameObject ball; // Assign the ball GameObject in the Unity Editor.
    public AudioClip collisionSound; // Assign this in the Unity Editor with your sound effect.

    private AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component attached to this GameObject.
        // Make sure to attach an AudioSource component to this GameObject in the Unity Editor.
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            Debug.LogWarning("CollisionHandler script expects an AudioSource component on the same GameObject.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ball)
        {
            BallSnapper ballSnapper = ball.GetComponent<BallSnapper>();
            if (ballSnapper != null)
            {
                // Disable the BallSnapper script on the ball.
                ballSnapper.enabled = false;
            }

            Collider ballCollider = ball.GetComponent<Collider>();
            if (ballCollider != null)
            {
                // Make the ball fall through the ground by disabling its collider.
                ballCollider.enabled = false;
            }

            // Play the collision sound effect if it's assigned and the AudioSource component exists.
            if (audioSource && collisionSound)
            {
                audioSource.PlayOneShot(collisionSound);
            }
        }
    }
}