using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollingText : MonoBehaviour
{
    public float scrollSpeed = 100.0f;
    public float textPositionBegin = 0;
    public float boundaryTextEnd = 0;

    private RectTransform textRectTransform;
    [SerializeField]
    TextMeshProUGUI bodyCopyText;

    [SerializeField] private bool isLooping = false;
    // We doin another Coroutine~
    void Start()
    {
        textRectTransform = gameObject.GetComponent<RectTransform>();
        StartCoroutine(AutoScrollText());
    }

    IEnumerator AutoScrollText()
    {
        while (textRectTransform.localPosition.y < boundaryTextEnd)
        {
            textRectTransform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
            if (textRectTransform.localPosition.y > boundaryTextEnd)
            {
                if (isLooping)
                {
                    textRectTransform.localPosition = Vector3.up * textPositionBegin;
                }
                else
                {
                    break;
                }
            }

            yield return null;
        }
    }
}
