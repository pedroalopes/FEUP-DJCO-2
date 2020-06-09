using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderAndTextUpdate : MonoBehaviour
{
    public Slider slider;
    public TMP_Text textMeshPro;

    public void Start()
    {
        slider.onValueChanged.AddListener(delegate {ValueChange(); });
        if (slider.wholeNumbers)
        {
            textMeshPro.text = "0\t";
        }
        else
        {
            textMeshPro.text = "0.0\t";
        }
        
    }
    
    public void ValueChange()
    {
        if (slider.wholeNumbers)
        {
            textMeshPro.text = slider.value.ToString() + "\t";
        }
        else
        {
            textMeshPro.text = slider.value.ToString("F1") + "\t";
        }
    }
}
