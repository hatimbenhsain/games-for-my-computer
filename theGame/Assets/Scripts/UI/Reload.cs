using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class Reload : MonoBehaviour
{
    public TMP_Text countdownText;  
    private GameObject countdownBackground;  
    public float inputTimeout = 60f;
    private float lastInputTime;
    private Coroutine countdownCoroutine;

    void Start()
    {
        countdownBackground = countdownText.transform.parent.gameObject;
        countdownBackground.SetActive(false);
        ResetLastInputTime();
    }

    void Update()
    {
        // if input
        if (Input.anyKey || Input.anyKeyDown)
        {
            ResetLastInputTime();
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
                countdownBackground.SetActive(false);
            }
        }

        // if no input for inputTimeout seconds
        if (Time.time - lastInputTime > inputTimeout && !Input.anyKey && !Input.anyKeyDown)
        {
            if (countdownCoroutine == null)
            {
                countdownBackground.SetActive(true);  
                countdownCoroutine = StartCoroutine(StartCountdown(10));
            }
        }
        // if input
        else if (Input.anyKey || Input.anyKeyDown)
        {
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
                countdownCoroutine = null;
                countdownBackground.SetActive(false);
            }
        }
    }

    private void ResetLastInputTime()
    {
        lastInputTime = Time.time;
    }

    private IEnumerator StartCountdown(int seconds)
    {
        while (seconds > 0)
        {
            countdownText.text = seconds.ToString();
            yield return new WaitForSeconds(1);
            seconds--;
        }

        countdownBackground.SetActive(false);  
        ActionAfterCountdown();
    }

    private void ActionAfterCountdown()
    {
        SceneManager.LoadScene("GameStart");
    }
}