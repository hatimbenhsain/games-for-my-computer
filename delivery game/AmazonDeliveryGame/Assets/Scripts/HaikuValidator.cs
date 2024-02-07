using UnityEngine;
using TMPro; // Namespace for TextMesh Pro
using UnityEngine.UI; // For using the Button component
using System.Text.RegularExpressions; // Namespace for Regex

public class HaikuValidatorThreeFields : MonoBehaviour
{
    public TMP_InputField inputFieldLine1; // The TMP_InputField component for the first line
    public TMP_InputField inputFieldLine2; // The TMP_InputField component for the second line
    public TMP_InputField inputFieldLine3; // The TMP_InputField component for the third line
    public Button submitButton; // Submit button to enable/disable based on validation

    // Use Start or Awake to initialize the fields to red indicating awaiting valid input
    private void Start()
    {
        submitButton.interactable = false; // Initially disable the submit button
        SetInputFieldColor(inputFieldLine1, false); // Set initial color to red
        SetInputFieldColor(inputFieldLine2, false); // Set initial color to red
        SetInputFieldColor(inputFieldLine3, false); // Set initial color to red
        
        // Add listeners to inputFields to validate text every time it changes
        inputFieldLine1.onValueChanged.AddListener(delegate { ValidateHaiku(); });
        inputFieldLine2.onValueChanged.AddListener(delegate { ValidateHaiku(); });
        inputFieldLine3.onValueChanged.AddListener(delegate { ValidateHaiku(); });
    }

    void ValidateHaiku()
    {
        // Count syllables in each line
        int syllablesLine1 = CountSyllables(inputFieldLine1.text);
        int syllablesLine2 = CountSyllables(inputFieldLine2.text);
        int syllablesLine3 = CountSyllables(inputFieldLine3.text);

        // Validate each line and set the color
        SetInputFieldColor(inputFieldLine1, syllablesLine1 == 5);
        SetInputFieldColor(inputFieldLine2, syllablesLine2 == 7);
        SetInputFieldColor(inputFieldLine3, syllablesLine3 == 5);

        // Check for the 5-7-5 syllable structure
        if (syllablesLine1 == 5 && syllablesLine2 == 7 && syllablesLine3 == 5)
        {
            submitButton.interactable = true; // Haiku is valid, enable the submit button
        }
        else
        {
            submitButton.interactable = false; // If checks fail, disable the submit button
        }
    }

    void SetInputFieldColor(TMP_InputField field, bool isValid)
    {
        Color color = isValid ? Color.green : Color.red;
        field.image.color = color; // Change the input field background color based on validity
    }

    int CountSyllables(string line)
    {
        int syllableCount = 0;
        string[] words = line.Split(' ');

        foreach (string word in words)
        {
            if (!string.IsNullOrWhiteSpace(word))
            {
                syllableCount += CountSyllablesInWord(word);
            }
        }

        return syllableCount;
    }

    int CountSyllablesInWord(string word)
    {
        word = word.ToLowerInvariant().Trim();
        if (word.Length <= 3) return 1; // Shortcut for short words

        word = Regex.Replace(word, "(?:[^laeiouy]es|ed|[^laeiouy]e)$", "");
        word = Regex.Replace(word, "^y", "");

        var matches = Regex.Matches(word, "[aeiouy]{1,2}");

        return matches.Count;
    }
}
