﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject door;

    public bool isOpened = false;
    private int objectsColliding = 0;

    private void OnCollisionEnter(Collision collision)
    {
        addCollision();
    }

    private void OnCollisionExit(Collision collision)
    {
        removeCollision();
    }

    public void addCollision()
    {
        objectsColliding++;
    }

    public void removeCollision()
    {
        objectsColliding--;
    }

    private void Update()
    {
        if(objectsColliding <= 0 && isOpened)
        {
            door.gameObject.transform.GetChild(0).position += new Vector3(0, 6, 0);
            door.gameObject.transform.GetChild(1).position += new Vector3(0, 6, 0);
            isOpened = false;
        } else if(!isOpened && objectsColliding > 0)
        {
            door.gameObject.transform.GetChild(0).position -= new Vector3(0, 6, 0);
            door.gameObject.transform.GetChild(1).position -= new Vector3(0, 6, 0);
            isOpened = true;
        }

    }
}
