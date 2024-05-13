using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseIcon : MonoBehaviour
{
    public float fadeDuration = 0.5f; 
    public float radius = 5f; 
    private Image image;
    private Vector2 originalPosition;
    private float angle; 

    void Start()
    {
        image = GetComponent<Image>();
        if (image == null)
            Debug.LogError("Image component is missing!");

        originalPosition = transform.position;
    }

    void Update()
    {
        // Circular motion
        angle += Time.deltaTime * 3; 
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        transform.position = originalPosition + new Vector2(x, y);
    }

    public void MouseFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    public void MouseFadeIn()
    {
        StartCoroutine(FadeIn());
    }


    public IEnumerator FadeIn()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }
    }
}
