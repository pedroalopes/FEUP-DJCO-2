using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

		CanvasScaler canvasScaler = pauseMenu.GetComponent<CanvasScaler>();
		canvasScaler.referenceResolution = new Vector2(userSettings.display.getScreenResolution().getWidth(), userSettings.display.getScreenResolution().getWidth());

		Camera camera = (Camera)player.GetComponentInChildren(typeof(Camera));
		camera.fieldOfView = userSettings.display.getScreenResolution().getWidth() / 23;
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
		Time.timeScale = 1f;
        userSettings.level.currentLevel = "EarthScene";
        ManageUserSettings.SaveUserSettings(userSettings);
        SceneManager.LoadScene("MainMenu");
    }
    
    public void Continue()
    {
		Time.timeScale = 1f;
		Component[] components = player.GetComponentsInChildren(typeof(CameraMovement));
		CameraMovement cameraMovement = (CameraMovement) components[0];
		cameraMovement.LockCursor();

        pauseMenu.SetActive(false);
        gameIsPaused = false;
    }
    
    public void Restart()
    {
		Time.timeScale = 1f;
        SceneManager.LoadScene(userSettings.level.currentLevel);
    }
    
    public void QuitGame() {
        Application.Quit();
    }
}
