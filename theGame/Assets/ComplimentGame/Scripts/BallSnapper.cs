using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSnapper : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject mazePanel;

    public int maxDistance;
        

    private bool isFixed;
    
    

    // Update is called once per frame
    void Update()
    {
        var cast =Physics.
            Raycast(transform.position, Vector3.down, maxDistance,1 <<  LayerMask.NameToLayer("MazePanel"));

        var rb = GetComponent<Rigidbody>();
        if (!cast)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            
            var dist = transform.position - mazePanel.transform.position;
            transform.position = new Vector3(transform.position.x, transform.position.y + dist.normalized.magnitude + 1,
                transform.position.z);

        }
        else
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.None;

        }
    }
}
