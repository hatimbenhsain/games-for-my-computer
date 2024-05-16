using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public string mainMenuScene;
    private CinemachineVirtualCamera playerCamera;
    private Transform playerCameraRoot; // The target position and rotation for the player
    private CursorLockMode myLockState;

    // Start is called before the first frame update
    void Start()
    {
        GameIsPaused = false;
        playerCamera = GameObject.Find("PlayerFollowCamera")?.GetComponent<CinemachineVirtualCamera>();
        playerCameraRoot = GameObject.Find("PlayerCameraRoot")?.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            myLockState = UnityEngine.Cursor.lockState;
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Resume()
    {
        // set timescale back to 1, unlock camera
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = myLockState;
        if (playerCamera != null)
        {
            playerCamera.Follow = playerCameraRoot;
        }

    }



    void Pause()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        // set timescale to 0, lock camera
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        UnityEngine.Cursor.visible = true;
        if (playerCamera != null)
        {
            playerCamera.Follow = null;
        }
    }
}
