using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public PlayerController player;
    public Image[] crosshairs = new Image[4];
    public Image[] symbols = new Image[4];
    
    // Update is called once per frame
    void Update()
    {
        switch (player.element)
        {
            case Element.Earth:
                SetCrosshairElement(0);
                break;
            case Element.Wind:
                SetCrosshairElement(1);
                break;
            case Element.Fire:
                SetCrosshairElement(2);
                break;
            case Element.Water:
                SetCrosshairElement(3);
                break;
        }
    }

    private void SetCrosshairElement(int index)
    {
        for(int i = 0; i < 4; i++)
        {
            if (i != index)
            {
                crosshairs[i].gameObject.SetActive(false);
                symbols[i].color = new Color32(140, 140,140,255);
            } else  {
                crosshairs[i].gameObject.SetActive(true);
                symbols[i].color = new Color32(255, 255, 255, 255);

            }
        }
    }

}
