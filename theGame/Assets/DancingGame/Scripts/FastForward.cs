using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FastForward : MonoBehaviour
{
    public TMP_Text fastForward;
    public GameObject pauseMenu;
    public bool canFastForward;
    // Start is called before the first frame update
    void Start()
    {
        canFastForward = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseMenu.activeSelf)
        {
            if (Input.GetMouseButton(1))
            {
                if (Input.GetMouseButton(0)) // Left mouse button held down
                {
                    if (canFastForward)
                    {
                        Time.timeScale = 10.0f;
                        fastForward.text = "Speed x10";
                    }
                    else
                    {
                        Time.timeScale = 1;
                    }

                }
                else // Only the right mouse button is held down
                {
                    if (canFastForward)
                    {
                        Time.timeScale = 3.0f;
                        fastForward.text = "Speed x3, left click to go crazy";
                    }
                    else
                    {
                        Time.timeScale = 1;
                    }
                }

            }
            else if (Input.GetMouseButtonUp(1))
            {
                Time.timeScale = 1;
                fastForward.text = "Right click to fast forward";
            }
        }
    }

    public void DeactivateFastForward()
    {
        canFastForward = false;
    }
}
