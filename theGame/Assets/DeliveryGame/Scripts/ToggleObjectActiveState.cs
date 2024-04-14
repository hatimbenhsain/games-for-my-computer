using UnityEngine;
using UnityEngine.UI; // Required for the Button component

public class ToggleObjectActiveState : MonoBehaviour
{
    public Button toggleButton; // The button to trigger the toggle
    public GameObject objectToToggle; // The GameObject to be toggled on and off

    void Start()
    {
        // Add a listener to the button's onClick event
        toggleButton.onClick.AddListener(ToggleObjectState);
    }

    void ToggleObjectState()
    {
        // If the object is not null, toggle its active state
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(!objectToToggle.activeSelf);

            //(try to) activate input field
            FieldSelector fs=objectToToggle.GetComponentInChildren<FieldSelector>();
            if(fs!=null){
                Debug.Log("found fs");
                fs.Activate();
            }else{
                Debug.Log("not found fs");
            }
        }
    }

    private void OnDestroy()
    {
        // Clean up the listener when the script is destroyed or the GameObject is deactivated
        toggleButton.onClick.RemoveListener(ToggleObjectState);
    }
}