using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneDJ : MonoBehaviour
{
    private GameObject DJCandyModel;
    private GameObject DJCandyHand;
    private bool isDJing;
    private bool isHandMoving;
    public GameObject leftHand;
    public GameObject rightHand;
    public float radius = 5.0f; // Radius of the movement circle
    public float verticalAdjustmentFactor = 0.1f;

    private Vector3 leftHandInitialPosition;
    private Vector3 rightHandInitialPosition;
    private Vector3 initialMousePosition;
    private Vector3 screenBounds;

    // Start is called before the first frame update
    void Start()
    {
        DJCandyModel = GameObject.Find("DancingCandy");
        DJCandyHand = GameObject.Find("djCandyArm");
        // Store initial positions
        leftHandInitialPosition = leftHand.transform.position;
        rightHandInitialPosition = rightHand.transform.position;

        initialMousePosition = Input.mousePosition;
        screenBounds = new Vector3(Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDJing)
        {
            DJCandyModel.SetActive(false);
            
        }
        else
        {
            DJCandyModel.SetActive(true);
        }
        if (isHandMoving)
        {
            DJCandyHand.SetActive(true);
            Vector3 mouseDelta = Input.mousePosition - initialMousePosition;
            mouseDelta = new Vector3(mouseDelta.x / screenBounds.x, mouseDelta.y / screenBounds.y, 0);

            // Update positions based on mouse movement, maintain within circular boundary
            UpdateHandPosition(ref leftHand, leftHandInitialPosition, -mouseDelta); // Invert mouse movement for left hand
            UpdateHandPosition(ref rightHand, rightHandInitialPosition, mouseDelta);
        }
        else
        {
            DJCandyHand.SetActive(false);
        }
    }

    public void ToggleDJ()
    {
        isDJing = !isDJing;
    }

    public void ToggleDJHand()
    {
        initialMousePosition = Input.mousePosition;
        isHandMoving = !isHandMoving;
    }
    void UpdateHandPosition(ref GameObject hand, Vector3 initialPosition, Vector3 mouseDelta)
    {
        // Calculate new position
        Vector3 newPosition = initialPosition + new Vector3(mouseDelta.x * radius * 2, 0, mouseDelta.y * radius * 2);

        // Check if within radius
        if ((newPosition - initialPosition).sqrMagnitude > radius * radius)
        {
            newPosition = initialPosition + (newPosition - initialPosition).normalized * radius;
        }

        // Adjust y based on z position: lower z results in higher y, and vice versa
        float yAdjustment = (initialPosition.z - newPosition.z) * verticalAdjustmentFactor;

        // Set the hand's position, including y adjustment
        hand.transform.position = new Vector3(newPosition.x, initialPosition.y + yAdjustment, newPosition.z);
    }
}
