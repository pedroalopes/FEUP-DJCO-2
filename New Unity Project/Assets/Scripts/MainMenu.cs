﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using static ManageUserSettings;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject canvas;

    private UserSettings userSettings;
    public void Start()
    {
        userSettings = ManageUserSettings.LoadUserSettings();
        CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
        canvasScaler.referenceResolution = new Vector2(userSettings.display.getScreenResolution().getWidth(), userSettings.display.getScreenResolution().getWidth());
    }
    public void PlayGame()
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
