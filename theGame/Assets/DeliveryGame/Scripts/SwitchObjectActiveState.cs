using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Button

public class SwitchObjectActiveStateWithBool : MonoBehaviour
{
    public GameObject objectToActivate; // GameObject to activate
    public Button buttonToPress; // Button that triggers the switch
    public bool keepParentActive; // Public bool to control the parent object's state
    public string nodeToCall="";

    private void Start()
    {
        // Ensure the button and method are properly linked
        if (buttonToPress != null)
        {
            buttonToPress.onClick.AddListener(SwitchActiveState); // Add SwitchActiveState method as a listener to the button's onClick event
        }
        else
        {
            Debug.LogWarning("ButtonToPress is not assigned in the Inspector.", this);
        }
    }

    public void SwitchActiveState()
    {
        if (objectToActivate != null)
        {
            // Activate the specified object
            objectToActivate.SetActive(true);
            
            // // (try to) Activate the first input field if there's one
            // FieldSelector fs=objectToActivate.GetComponentInChildren<FieldSelector>();
            // if(fs!=null){
            //     Debug.Log("found fs");
            //     fs.Activate();
            // }else{
            //     Debug.Log("not found fs");
            // }
        }
        else
        {
            Debug.LogWarning("ObjectToActivate is not assigned in the Inspector.", this);
        }

        if(nodeToCall!=""){
            CallNode();
        }

        // Check the boolean to decide on parent GameObject's active state
        if (transform.parent != null)
        {
            // If keepParentActive is true, do not deactivate the parent GameObject
            // If false, deactivate the parent GameObject
            transform.parent.gameObject.SetActive(keepParentActive);
        }
        else
        {
            Debug.LogError("This GameObject has no parent.", this);
        }


    }

    public void CallNode(){
        FindObjectOfType<DeliveryGame>().Bark(nodeToCall);
    }
}