using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleEnter : MonoBehaviour
{
    // Start is called before the first frame update
    private Coroutine rippleRoutine;
    [SerializeField] private float rippleTime = 1.5f;
    [SerializeField] private float maxRippleStrength = 0.75f;

    private void Start()
    {
        var mat = GetComponent<Renderer>().material;
        mat.SetFloat("_RippleStrength", 0);
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

            rippleRoutine = StartCoroutine(DoRipple(mat));
        }
    }

    private IEnumerator DoRipple(Material mat)
    {
        for (float t = 0.0f; t < rippleTime; t += Time.deltaTime)
        {
            mat.SetFloat("_RippleStrength", maxRippleStrength * (1.0f - t / rippleTime));
            yield return null;
        }
    }
}
