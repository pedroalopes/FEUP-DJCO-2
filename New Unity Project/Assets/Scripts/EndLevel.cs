using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    public Animator transitionAnimation;
    public string sceneName;

    public Transform levelTransition;
    private LevelLoader loader;

    void Start()
    {
        loader = levelTransition.GetComponent<LevelLoader>();
    }
    public void ChangeLevel()
    {
        UserSettings userSettings = ManageUserSettings.LoadUserSettings();
        userSettings.level.currentLevel = sceneName;
        ManageUserSettings.SaveUserSettings(userSettings);
        loader.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

}