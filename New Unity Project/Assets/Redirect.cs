using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Redirect : MonoBehaviour
{
    public CameraMovement movement;
    public Animator animator;

    void Start()
    {
        movement.UnlockCursor();
    }
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
