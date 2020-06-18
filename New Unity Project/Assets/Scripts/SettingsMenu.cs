﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
 using UnityEngine.UI;
 using TMPro;

using FMODUnity;

public class SettingsMenu : MonoBehaviour
{
    private UserSettings userSettings;
	public GameObject ambientSound;
	public GameObject player;
	public GameObject directionalLight;

    public void Start()
    {
        userSettings = ManageUserSettings.LoadUserSettings();

        setDisplay();
        setSounds();
        setControls();
    }

    private void setDisplay()
    {
        Component[] components = gameObject.GetComponentsInChildren(typeof(Slider));
        
        foreach (Slider slider in components)
        {
            slider.value = userSettings.display.getDisplay(slider.name);
            
            Component component = slider.gameObject.GetComponentInChildren(typeof(TextMeshProUGUI));
            TextMeshProUGUI tmpText = (TextMeshProUGUI) component;
            tmpText.text = slider.value.ToString("F1");
        }

		components = gameObject.GetComponentsInChildren(typeof(TMP_Dropdown));
        
        foreach (TMP_Dropdown tmp_dropdown in components)
        {
			tmp_dropdown.value = userSettings.display.getScreenResolution().position;
        }

		if(!userSettings.sound.getSound("ambientMusic") && ambientSound.GetComponent<StudioEventEmitter>().IsPlaying()) {
			ambientSound.GetComponent<StudioEventEmitter>().Stop();
		} else if(userSettings.sound.getSound("ambientMusic") && !ambientSound.GetComponent<StudioEventEmitter>().IsPlaying()) {
			ambientSound.GetComponent<StudioEventEmitter>().Play();
		}

		if(!userSettings.sound.getSound("playerSounds")) {
			player.GetComponent<PlayerController>().soundEnabled = false;
		} else {
			player.GetComponent<PlayerController>().soundEnabled = true;
		}
    }

    private void setSounds()
    {
        Component[] components = gameObject.GetComponentsInChildren(typeof(Toggle));

        foreach (Toggle toggle in components)
        {
            toggle.isOn = userSettings.sound.getSound(toggle.name);
        }
    }

    private void setControls()
    {
        Component[] components = gameObject.GetComponentsInChildren(typeof(InputField));
        
        foreach (InputField inputField in components)
        {
			KeyCode keyCode = userSettings.controls.getControl(inputField.name);
			string str = "";

			if(keyCode == KeyCode.Space) {
				str = ".";
			} else if(keyCode == KeyCode.LeftShift) {
			str = ",";
			} else if(keyCode == KeyCode.Mouse0) {
				str = "~";
			} else {
				str = keyCode.ToString();
			}
        	inputField.text = str;
        }
    }
    public void Apply() 
    {
		if(!userSettings.sound.getSound("ambientMusic") && ambientSound.GetComponent<StudioEventEmitter>().IsPlaying()) {
			ambientSound.GetComponent<StudioEventEmitter>().Stop();
		} else if(userSettings.sound.getSound("ambientMusic") && !ambientSound.GetComponent<StudioEventEmitter>().IsPlaying()) {
			ambientSound.GetComponent<StudioEventEmitter>().Play();
		}

		if(!userSettings.sound.getSound("playerSounds")) {
			player.GetComponent<PlayerController>().soundEnabled = false;
		} else {
			player.GetComponent<PlayerController>().soundEnabled = true;
		}
		Screen.SetResolution(userSettings.display.getScreenResolution().getWidth(), userSettings.display.getScreenResolution().getHeight(), true);

		Debug.Log("**************************");

		// Camera camera = (Camera)player.GetComponentInChildren(typeof(Camera));
		// camera.fieldOfView = userSettings.display.getScreenResolution().getWidth() / 23;

		Light light = (Light)directionalLight.GetComponent(typeof(Light));
		light.intensity = userSettings.display.getDisplay("ambientLight") + 0.5f;

        ManageUserSettings.SaveUserSettings(userSettings);
    }

	public void ChangedResolution(TMP_Dropdown change) {		
		userSettings.display.setScreenResolution(change.options[change.value].text, change.value);
	}

	public void ChangedAmbientLight(Slider slider) {		
		userSettings.display.setDisplay(slider.name, slider.value);
	}

	public void ChangedSound(Toggle toggle) {		
		userSettings.sound.setSound(toggle.name, toggle.isOn);
	}

	public void ChangedControl(InputField inputField) {
		KeyCode keyCode;
		if(inputField.text == ".") {
			keyCode = KeyCode.Space;
		} else if(inputField.text == ",") {
			keyCode = KeyCode.LeftShift;
		} else if(inputField.text == "~") {
			keyCode = KeyCode.Mouse0;
		} else {
			keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), inputField.text);
		}
		userSettings.controls.setControl(inputField.name, keyCode);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
