using UnityEngine;
using TMPro; 
using UnityEngine.UI; 

public class InputFieldCheck : MonoBehaviour
{
    public TMP_InputField inputField; // The TMP_InputField component
    public Button submitButton; // The Button component that should be enabled/disabled
    public string correctString; // Publicly assignable variable to hold the correct string

    private void Start()
    {
        correctString = correctString.ToLower();
        UpdateButtonState(); // Initial check in case you need the button to be disabled from the start
        inputField.onValueChanged.AddListener(delegate { UpdateButtonState(); }); // Add listener for input field changes
    }

    void UpdateButtonState()
    {
        var inputTxt = inputField.text.ToLower();
        
        if (inputTxt == correctString)
        {
            inputField.image.color = Color.green; // Change input field color to green
            submitButton.interactable = true; // Make the button pressable
        }
        else
        {
            inputField.image.color = Color.red; // Change input field color to red
            submitButton.interactable = false; // Make the button not pressable
        }
    }
}