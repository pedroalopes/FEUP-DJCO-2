using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;

using static ManageUserSettings;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject canvas;
    public GameObject continueButton;
    private UserSettings userSettings;
    public void Start()
    {
        userSettings = ManageUserSettings.LoadUserSettings();
        
        Screen.SetResolution(userSettings.display.getScreenResolution().getWidth(), userSettings.display.getScreenResolution().getHeight(), true);

        // CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
        // canvasScaler.referenceResolution = new Vector2(userSettings.display.getScreenResolution().getWidth(), userSettings.display.getScreenResolution().getWidth());
        if (userSettings.level.currentLevel == "MainMenu" || userSettings.level.currentLevel == "")
        {
            userSettings.level.currentLevel = "EarthScene";
            ManageUserSettings.SaveUserSettings(userSettings);
        }
        
        if (userSettings.level.currentLevel == "EarthScene")
        {
            continueButton.SetActive(false);
        }
        else
        {
            continueButton.SetActive(true);
        }
    }

    public void Update()
    {
        userSettings = ManageUserSettings.LoadUserSettings();
        
        CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
        canvasScaler.referenceResolution = new Vector2(userSettings.display.getScreenResolution().getWidth(), userSettings.display.getScreenResolution().getWidth());
    }
    
    public void PlayGame()
    {
        userSettings.level.currentLevel = "EarthScene";
        ManageUserSettings.SaveUserSettings(userSettings);
        SceneManager.LoadScene("EarthScene");
    }
    
    public void Continue()
    {
        SceneManager.LoadScene(userSettings.level.currentLevel);
    }

    public void showSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
