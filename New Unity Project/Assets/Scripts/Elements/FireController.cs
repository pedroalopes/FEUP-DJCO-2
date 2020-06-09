using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public Transform firePrefab;
    public GameObject firePoint;
    private Transform fireObject;

    public void handleStart()
    {
        if (fireObject == null)
        {
            fireObject = Instantiate(firePrefab, Camera.main.transform.position + (Camera.main.transform.forward * 1), transform.rotation);
            fireObject.SetParent(firePoint.transform);
        }
    }
    public void handleCharge()
    {

        if (fireObject.localScale.x < 1)
        {
            fireObject.localScale = fireObject.localScale + new Vector3(0.01f, 0.01f, 0.01f);
            fireObject.GetComponent<FireObjectScript>().addDamage();
            fireObject.GetComponent<FireObjectScript>().removeForce();
        }

    }

    public void handleRelease(RaycastHit hit)
    {
        Rigidbody rb = fireObject.gameObject.GetComponent<Rigidbody>();
        Vector3 shoot = (hit.point - fireObject.position).normalized;

        rb.AddForce(shoot * (int)fireObject.GetComponent<FireObjectScript>().getForce());
        fireObject.SetParent(null);
        fireObject = null;
    }
}
