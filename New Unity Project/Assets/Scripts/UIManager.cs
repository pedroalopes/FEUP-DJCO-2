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
                if(player.AllowedElements[0])
                    SetCrosshairElement(0);
                break;
            case Element.Water:
                if (player.AllowedElements[1])
                    SetCrosshairElement(1);
                break;
            case Element.Wind:
                if (player.AllowedElements[2])
                    SetCrosshairElement(2);
                break;
            case Element.Fire:
                if (player.AllowedElements[3])
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
