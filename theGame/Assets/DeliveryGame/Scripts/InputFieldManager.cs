using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // Required for Image component

public class InputFieldManager : MonoBehaviour
{
    public static InputFieldManager Instance { get; private set; }

    private TMP_InputField activeField;
    private Color defaultColor = Color.white;  // Default color for input fields
    private Color activeColor = new Color(1f, 0.7f, 0.7f);  // Light red color

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void RegisterField(TMP_InputField field)
    {
        // Optionally keep a list of fields if needed
        // Initialize fields with default color
        var image = field.GetComponent<Image>();
        if (image != null)
        {
            image.color = defaultColor;
        }
    }

    public void SetActiveField(TMP_InputField field)
    {
        // Reset the color of the previously active field, if it exists
        if (activeField != null)
        {
            var previousImage = activeField.GetComponent<Image>();
            if (previousImage != null)
            {
                previousImage.color = defaultColor;
            }
        }

        // Set the new active field and change its color
        activeField = field;
        var currentImage = activeField.GetComponent<Image>();
        if (currentImage != null)
        {
            currentImage.color = activeColor;
        }

        Debug.Log("Active field set: " + field.name);
    }

    public void ClearActiveField()
    {
        if (activeField != null)
        {
            var image = activeField.GetComponent<Image>();
            if (image != null)
            {
                image.color = defaultColor;
            }
        }
        activeField = null;
    }

    public TMP_InputField GetActiveField()
    {
        return activeField;
    }
}
