using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasdMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f , verticalInput);
        
        movement.Normalize();

        rb.velocity = movement * moveSpeed;
    }
}
