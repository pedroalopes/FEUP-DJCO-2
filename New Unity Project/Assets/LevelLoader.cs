using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelLoader : MonoBehaviour
{
    AsyncOperation async;
    // public Button button;
    public Animator loadingScreenAnimator;
    public CameraMovement cameraMovement;
    public GameObject transitionContent;
    public GameObject backgroundPanel;
    public GameObject UI;

    void Start()
    {
        transitionContent.gameObject.SetActive(false);
        StartCoroutine(deactivateBackground());
        // button.gameObject.SetActive(false);

    }
    public void LoadLevel(int sceneIndex)
    {
        UI.gameObject.SetActive(false);
        transitionContent.gameObject.SetActive(true);
        backgroundPanel.gameObject.SetActive(true);

        // button.gameObject.SetActive(true);
        loadingScreenAnimator.SetTrigger("Start");
        cameraMovement.UnlockCursor();
    }

    IEnumerator deactivateBackground()
    {
        yield return new WaitForSeconds(0.9f);
        backgroundPanel.gameObject.SetActive(false);
    }

    public void NextScene()
    {
        StartCoroutine(LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1));
    }
    IEnumerator LoadNextScene(int sceneIndex)
    {
        int indexToLoad = sceneIndex;

        if (indexToLoad == SceneManager.sceneCountInBuildSettings)
            indexToLoad = 0;

        loadingScreenAnimator.SetTrigger("ActivateLevel");
        yield return new WaitForSeconds(3f);
        UserSettings userSettings = ManageUserSettings.LoadUserSettings();
        userSettings.level.currentLevel = SceneManager.GetSceneByBuildIndex(indexToLoad).name;
        ManageUserSettings.SaveUserSettings(userSettings);

        SceneManager.LoadScene(indexToLoad);
    }

    /*IEnumerator LoadAsync(int sceneIndex)
    {
        async = SceneManager.LoadSceneAsync(sceneIndex);
        async.allowSceneActivation = false;

        while (async.progress < 0.9)
        {
            Debug.Log(async.progress);
            yield return null;
        }

        button.gameObject.SetActive(true);
    }

    public void ActivateScene()
    {
        cameraMovement.LockCursor();
        loadingScreenAnimator.SetTrigger("ActivateLevel");
        async.allowSceneActivation = true;
    }*/
}
