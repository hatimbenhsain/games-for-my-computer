using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class RippleEnter : MonoBehaviour
{
    // handles the ripple effect and camera transition when enters the ripple
    // Start is called before the first frame update
    private Coroutine rippleRoutine;
    [SerializeField] private float rippleTime = 1.5f;
    [SerializeField] private float maxRippleStrength = 0.75f;
    // variables for the camera transition
    public CinemachineVirtualCamera mainCamera;
    public Transform targetTransform; // The target position and rotation for the camera
    public float transitionSpeed = 1.0f;
    private bool isTransitioning = false;

    private void Start()
    {
        var mat = GetComponent<Renderer>().material;
        mat.SetFloat("_RippleStrength", 0);
    }

    void Update()
    {
        if (isTransitioning)
        {
            // Interpolate camera's position
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetTransform.position, transitionSpeed * Time.deltaTime);

            // Interpolate camera's rotation
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, targetTransform.rotation, transitionSpeed * Time.deltaTime);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player entered trigger");

            // Use the position of the player as the ripple center
            // You might want to adjust the position to be closer to the surface where the ripple should appear
            Vector3 rippleCenter = other.transform.position;

            var mat = GetComponent<Renderer>().material;
            mat.SetVector("_RippleCenter", new Vector4(rippleCenter.x, rippleCenter.y + 1, rippleCenter.z, 0));

            if (rippleRoutine != null)
            {
                StopCoroutine(rippleRoutine);
            }
            // ripple coroutine
            rippleRoutine = StartCoroutine(DoRipple(mat));

            // set the cinemachine transition
            isTransitioning = true;
            mainCamera.Follow = null;
            //mainCamera.LookAt = null;
        }
    }

    private IEnumerator DoRipple(Material mat)
    {
        // for ripple time, initiate the ripple and strength goes down
        for (float t = 0.0f; t < rippleTime; t += Time.deltaTime)
        {
            mat.SetFloat("_RippleStrength", maxRippleStrength * (1.0f - t / rippleTime));
            yield return null;
        }
    }
}
