﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.UI;
 using TMPro;

[System.Serializable]
public class Display
{
    public float ambientLight;
	public ScreenResolution screenResolution;
    
    public float getDisplay(string name) {
        if(name == "ambientLight") {
            return ambientLight;
        }
        return 0.0f;
    }

	public ScreenResolution getScreenResolution() {
		return screenResolution;
	}
    
    public void setDisplay(string name, float value) {
        if(name == "ambientLight") {
            ambientLight = value;
        }
    }

	public void setScreenResolution(string str, int position) {
		string[] arr = str.Split('x');
		int width = int.Parse(arr[0]);
		int height = int.Parse(arr[1]);
		screenResolution = new ScreenResolution(width, height, position);
	}
}