using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
    
using static ManageUserSettings;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;

    private UserSettings userSettings;
    public void Start()
    {
        Debug.Log("Start main menu");
        userSettings = ManageUserSettings.LoadUserSettings();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void showSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
}
