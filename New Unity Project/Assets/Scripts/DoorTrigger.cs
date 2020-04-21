using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject door;

    bool isOpened = false;


    private void OnCollisionEnter(Collision collision)
    {
        door.transform.position += new Vector3(0, -6, 0);
    }

    private void OnCollisionExit(Collision collision)
    {
        door.transform.position += new Vector3(0, 6, 0);
    }

}
