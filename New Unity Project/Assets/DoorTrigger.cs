using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject door;
    [SerializeField]
    GameObject door2 = null;
    public bool startOpen = false;
    private bool isOpened = false;
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
        if(!startOpen)
        {
            handleStartClosed();
        } else
        {
            handleStartOpen();
        }
    }

    private void handleStartClosed()
    {
        if (objectsColliding <= 0 && isOpened)
        {
            door.gameObject.transform.GetChild(0).position += new Vector3(0, 6, 0);
            door.gameObject.transform.GetChild(1).position += new Vector3(0, 6, 0);
            isOpened = false;
            if(door2 != null)
            {
                door2.gameObject.transform.GetChild(0).position += new Vector3(0, 6, 0);
                door2.gameObject.transform.GetChild(1).position += new Vector3(0, 6, 0);
            }
        }
        else if (!isOpened && objectsColliding > 0)
        {
            door.gameObject.transform.GetChild(0).position -= new Vector3(0, 6, 0);
            door.gameObject.transform.GetChild(1).position -= new Vector3(0, 6, 0);
            if(door2 != null)
            {
                door.gameObject.transform.GetChild(0).position -= new Vector3(0, 6, 0);
                door.gameObject.transform.GetChild(1).position -= new Vector3(0, 6, 0);
            }
            isOpened = true;
        }
    }

    private void handleStartOpen()
    {
        if (objectsColliding <= 0 && !isOpened)
        {
            door.gameObject.transform.GetChild(0).position -= new Vector3(0, 6, 0);
            door.gameObject.transform.GetChild(1).position -= new Vector3(0, 6, 0);
            isOpened = true;
        }
        else if (isOpened && objectsColliding > 0)
        {
            door.gameObject.transform.GetChild(0).position += new Vector3(0, 6, 0);
            door.gameObject.transform.GetChild(1).position += new Vector3(0, 6, 0);
            isOpened = false;
        }
    }
}
