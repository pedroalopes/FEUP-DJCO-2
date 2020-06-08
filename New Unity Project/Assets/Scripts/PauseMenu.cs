using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
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
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    
    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void Continue()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
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
