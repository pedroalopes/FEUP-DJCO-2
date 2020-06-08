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

        Debug.Log(userSettings.sound.ambientMusic);

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
            Debug.Log(tmpText.name);
            Debug.Log(tmpText.text);
            tmpText.text = slider.value.ToString("F1");
            slider.onValueChanged.AddListener(delegate {DisplayChanged(slider); });
        }
    }
    
    public void DisplayChanged(Slider slider)
    {
        userSettings.display.setDisplay(slider.name, slider.value);
    }

    private void setSounds()
    {
        Component[] components = gameObject.GetComponentsInChildren(typeof(Toggle));

        foreach (Toggle toggle in components)
        {
            toggle.isOn = userSettings.sound.getSound(toggle.name);
            toggle.onValueChanged.AddListener(delegate {SoundChanged(toggle); });
        }
    }
    public void SoundChanged(Toggle toggle)
    {
        userSettings.sound.setSound(toggle.name, toggle.isOn);
    }

    private void setControls()
    {
        Component[] components = gameObject.GetComponentsInChildren(typeof(InputField));
        
        foreach (InputField inputField in components)
        {
            inputField.text = userSettings.controls.getControl(inputField.name);
            inputField.onValueChanged.AddListener(delegate {ControlChanged(inputField); });
        }
    }

    private void ControlChanged(InputField inputField)
    {
        userSettings.controls.setControl(inputField.name, inputField.text);
    }

    public void Apply() 
    {
        ManageUserSettings.SaveUserSettings(userSettings);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
