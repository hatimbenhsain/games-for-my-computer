using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stranger2TextBoxMove : MonoBehaviour
{
    public Button clickButton;
    public GameObject objectToMove1;
    public GameObject objectToMove2;
    public Button buttonToHide; // Declare the button you want to hide
    public float targetYCoordinate = 500f;
    public float targetXCoordinate = 100f;

    void Start()
    {
        // Attach the button click event to your custom method
        clickButton.onClick.AddListener(OnClick);
    }

    // Custom method to handle button click
    void OnClick()
    {
        // Hide the clicked button
        buttonToHide.gameObject.SetActive(false);

        // Move the assigned objects to the target Y coordinate
        if (objectToMove1 != null)
            objectToMove1.transform.position = new Vector3(targetXCoordinate, targetYCoordinate, objectToMove1.transform.position.z);

        if (objectToMove2 != null)
            objectToMove2.transform.position = new Vector3(targetXCoordinate, targetYCoordinate, objectToMove2.transform.position.z);
    }
}
