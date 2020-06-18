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
    public GameObject player;
    public CameraMovement cameraMovement;
    public GameObject transitionContent;
    public GameObject backgroundPanel;
    public GameObject UI;
    private PlayerController controller;

    void Start()
    {
        controller = player.GetComponent<PlayerController>();
        controller.FreezePlayer();
        UI.gameObject.GetComponent<UIManager>().setCanvasInvisible();
        StartCoroutine(waitToEnableCursor());

    }

    IEnumerator waitToEnableCursor()
    {
        yield return new WaitForSeconds(1f);
        cameraMovement.UnlockCursor();
    }

    public void ContinueToLevel()
    {
        loadingScreenAnimator.SetTrigger("ContinueToLevel");
        StartCoroutine(setCanvasActive());
        cameraMovement.LockCursor();
    }

    IEnumerator setCanvasActive()
    {
        yield return new WaitForSeconds(3f);
        backgroundPanel.gameObject.SetActive(false);
        UI.gameObject.GetComponent<UIManager>().setCanvasVisible();
        controller.UnfreezePlayer();
    }

    public void LoadLevel(int sceneIndex)
    {
        backgroundPanel.gameObject.SetActive(true);
        loadingScreenAnimator.SetTrigger("EndLevel");
        NextScene();
    }

    public void NextScene()
    {
        controller.FreezePlayer();
        StartCoroutine(LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1));
    }
    IEnumerator LoadNextScene(int sceneIndex)
    {
        int indexToLoad = sceneIndex;

        if (indexToLoad == SceneManager.sceneCountInBuildSettings)
            indexToLoad = 0;

        yield return new WaitForSeconds(3f);
        // UserSettings userSettings = ManageUserSettings.LoadUserSettings();
        // userSettings.level.currentLevel = SceneManager.GetSceneByBuildIndex(indexToLoad).name;
        // ManageUserSettings.SaveUserSettings(userSettings);

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
