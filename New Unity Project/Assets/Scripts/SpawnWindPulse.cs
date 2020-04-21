using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWindPulse : MonoBehaviour
{
    public GameObject firePoint;
    public GameObject pulse;
    public RotateToMouse rotateToMouse;
    public CameraMovement cameraMovement;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SpawnProjectile();
        }
    }

    void SpawnProjectile()
    {
        GameObject obj;

        if (firePoint != null)
        {
            obj = Instantiate(pulse, firePoint.transform.position, Quaternion.identity);
            if (rotateToMouse != null)
            {
                obj.transform.localRotation = rotateToMouse.getRotation();
            }
        }
        else
        {
            Debug.Log("No firepoint");
        }
    }
}
