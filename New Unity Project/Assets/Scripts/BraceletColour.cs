using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BraceletColour : MonoBehaviour
{
    public Material[] materials = new Material[4];
    public PlayerController controller;

    // Update is called once per frame
    void Update()
    {
        switch (controller.element)
        {
            case Element.Earth:
                gameObject.GetComponent<Renderer>().material = materials[0];
                break;
            case Element.Water:
                gameObject.GetComponent<Renderer>().material = materials[1];
                break;
            case Element.Wind:
                gameObject.GetComponent<Renderer>().material = materials[2];
                break;
            case Element.Fire:
                gameObject.GetComponent<Renderer>().material = materials[3];
                break;

        }
    }
}
