using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform destination;
    private bool pickedUp = false;

    public void Interact()
    {
        if (!pickedUp)
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().freezeRotation = true;
        } else
        {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().freezeRotation = false;
        }
        pickedUp = !pickedUp;


    }

    private void Update()
    {
        if (pickedUp)
        {
            this.transform.position = destination.position;
        }
    }
}
