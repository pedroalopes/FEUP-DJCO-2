using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject player;
    private UserSettings userSettings;
    public static bool gameIsPaused = false;
    public void Start()
    {
        pauseMenu.SetActive(false);
        userSettings = ManageUserSettings.LoadUserSettings();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            if(gameIsPaused) {
                Continue();
            } else {
                Pause();
            }
        }
    }

    public void Pause()
    {
		Component[] components = player.GetComponentsInChildren(typeof(CameraMovement));
		CameraMovement cameraMovement = (CameraMovement) components[0];
		cameraMovement.UnlockCursor();

		pauseMenu.SetActive(true);
		gameIsPaused = true;
		Time.timeScale = 0f;
    }
    
    public void NewGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void Continue()
    {
Debug.Log("continue");
		Time.timeScale = 1f;
		Component[] components = player.GetComponentsInChildren(typeof(CameraMovement));
		CameraMovement cameraMovement = (CameraMovement) components[0];
		cameraMovement.LockCursor();

        pauseMenu.SetActive(false);
        gameIsPaused = false;
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(userSettings.level.currentLevel);
    }
    
    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
}
