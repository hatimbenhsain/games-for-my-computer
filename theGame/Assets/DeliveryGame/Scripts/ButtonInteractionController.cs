using UnityEngine;
using UnityEngine.UI; // Necessary for working with UI elements like Buttons

public class ButtonInteractionController : MonoBehaviour
{
    public Button triggerButton; // The button that needs to be pressed first
    public Button targetButton; // The button that will become pressable after the triggerButton is pressed

    void Start()
    {
        // Initially disable the targetButton
        targetButton.interactable = false;

        // Add a listener to the triggerButton so that when it's clicked, it will call the MakeTargetButtonInteractable method
        triggerButton.onClick.AddListener(MakeTargetButtonInteractable);
    }

    void MakeTargetButtonInteractable()
    {
        // Make the targetButton interactable
        targetButton.interactable = true;
    }
}