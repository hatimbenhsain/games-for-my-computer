using UnityEngine;
using UnityEngine.EventSystems;  // Required for UI event handling

public class EnableObjectOnHover : MonoBehaviour, IPointerEnterHandler
{
    public GameObject objectToEnable;  // Assign this in the inspector with the object you want to enable
    private bool hasHovered = false;   // To keep track if the hover has happened

    // This function is called when the mouse pointer enters the button area
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!hasHovered && objectToEnable != null)
        {
            objectToEnable.SetActive(true); // Enable the assigned object
            hasHovered = true;             // Ensure it only happens once
        }
    }
}
