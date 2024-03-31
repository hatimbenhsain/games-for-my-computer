using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainThatTalks : MonoBehaviour
{
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( _animator != null )
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _animator.SetTrigger("Rain");
            }
        }
    }
}
