using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    public Animator transitionAnimation;
    public string sceneName;
    public void ChangeLevel()
    {
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        transitionAnimation.SetTrigger("end");
        yield return new WaitForSeconds(1.5f);
        UserSettings userSettings = ManageUserSettings.LoadUserSettings();
        userSettings.level.currentLevel = sceneName;
        ManageUserSettings.SaveUserSettings(userSettings);
        SceneManager.LoadScene(sceneName);
    }
}