using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections; // Needed for IEnumerator

public class ChangingNumber : MonoBehaviour
{
    public TMP_InputField inputField; // The TMP_InputField for user input
    public TMP_InputField displayField; // The TMP_InputField to display the correct string
    public Button submitButton; // The Button component that should be enabled/disabled
    private string correctString; // Variable to hold the correct string, no longer publicly assignable
    public float changeInterval = 10f; // Time in seconds after which the correct string changes, publicly assignable

    private void Start()
    {
        StartCoroutine(ChangeCorrectStringPeriodically());
        UpdateButtonState(); // Initial check in case you need the button to be disabled from the start
        inputField.onValueChanged.AddListener(delegate { UpdateButtonState(); }); // Add listener for input field changes
    }

    IEnumerator ChangeCorrectStringPeriodically()
    {
        while (true)
        {
            correctString = GenerateRandomNumber(5);
            if (displayField != null)
            {
                displayField.text = correctString; // Display the correct string in the assigned TMP_InputField
            }
            yield return new WaitForSeconds(changeInterval); // Wait for the specified interval before changing the number again
        }
    }

    string GenerateRandomNumber(int length)
    {
        string randomNumber = "";
        for (int i = 0; i < length; i++)
        {
            randomNumber += Random.Range(0, 10).ToString(); // Generate a random digit and add it to the string
        }
        return randomNumber;
    }

    void UpdateButtonState()
    {
        if (inputField.text == correctString)
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
