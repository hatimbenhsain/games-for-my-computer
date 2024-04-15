using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarGame : MonoBehaviour
{
    public Slider progressBar;
    public Button pressButton;
    public AudioClip completionSound;
    public AudioClip alertSound;
    private AudioSource audioSource;

    public Image handleImage; // Reference to the Image component of the handle
    public Sprite normalSprite; // Sprite for normal state
    public Sprite cheerSprite; // Sprite for cheer state

    public int minButtonPresses = 10;
    public int maxButtonPresses = 25;
    private int pressesRequired;
    private int currentPressCount;

    public float fillSpeed = 0.0167f; // Public variable to adjust the fill speed
    private float nextCheerTime = 0f; // Time for the next cheer event
    private bool isProgressing = true; // Tracks if the progress bar is currently progressing

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pressButton.onClick.AddListener(ButtonPressed);
        ResetCheerTimer();
        handleImage.sprite = normalSprite; // Start with the normal sprite
        StartCoroutine(FillProgress());
    }

    private void ResetCheerTimer()
    {
        nextCheerTime = Time.time + Random.Range(5f, 10f); // Sets the next cheer time
    }

    private IEnumerator FillProgress()
    {
        while (progressBar.value < 1.0f)
        {
            if (isProgressing)
            {
                progressBar.value += fillSpeed * Time.deltaTime;
            }
            if (Time.time >= nextCheerTime && isProgressing)
            {
                isProgressing = false;
                pressesRequired = Random.Range(minButtonPresses, maxButtonPresses + 1);
                currentPressCount = 0; // Reset count each time we enter cheer state
                handleImage.sprite = cheerSprite; // Change to cheer sprite
                audioSource.PlayOneShot(alertSound);

                yield return new WaitUntil(() => currentPressCount >= pressesRequired);

                handleImage.sprite = normalSprite; // Change back to normal sprite
                ResetCheerTimer();
                isProgressing = true; // Resume progression after conditions are met
            }
            yield return null;
        }
        OnCompletion();
    }

    private void ButtonPressed()
    {
        if (!isProgressing) // Check if the progress is currently paused
        {
            currentPressCount++;
        }
    }

    private void OnCompletion()
    {
        audioSource.PlayOneShot(completionSound);
        FindObjectOfType<DeliveryGame>().DropPackage(); // Static call to DropPackage method
        gameObject.SetActive(false);
    }

    
}
