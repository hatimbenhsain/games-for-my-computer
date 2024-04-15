using UnityEngine;
using UnityEngine.UI;

public class ButtonStayPressed : MonoBehaviour
{
    public Button button;
    public Color pressedColor = Color.gray; // Color when button is pressed

    private Image buttonImage;
    private bool isPressed = false;

    void Start()
    {
        buttonImage = button.GetComponent<Image>();
        button.onClick.AddListener(ToggleButton);
    }

    private void ToggleButton()
    {
        if (!isPressed)
        {
            buttonImage.color = pressedColor; // Change color to indicate "pressed" state
            isPressed = true;
            button.interactable = false; // Optionally make button non-interactable after pressed
        }
    }
}