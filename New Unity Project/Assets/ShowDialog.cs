﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDialog : MonoBehaviour
{
    public GameObject PlayerUI;
    public string text;

    private bool shown = false;
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && !shown)
        {
            shown = PlayerUI.GetComponent<UIManager>().ShowDialog(text);
        }
    }
}