﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.UI;
 using TMPro;

public class SettingsMenu : MonoBehaviour
{
    private UserSettings userSettings;
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
            inputField.text = userSettings.controls.getControl(inputField.name);
        }
    }
    public void Apply() 
    {
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
		userSettings.controls.setControl(inputField.name, inputField.text);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
