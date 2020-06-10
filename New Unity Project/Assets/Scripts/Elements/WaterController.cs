using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    public Transform dropPrefab;
    private Transform waterObject;
    private Vector3 vecX;
    private Vector3 vecY;
    private Vector3 vecZ;
    private float dropCooldown;
    void Start()
    {
        vecX = new Vector3(-0.7f, 0, 0);
        vecY = new Vector3(0, -0.7f, 0);
        vecZ = new Vector3(0, 0, -0.7f);
        dropCooldown = 0;
    }

    public void handleStart(RaycastHit hit)
    {
        if (Time.time < dropCooldown)
        {
            Debug.Log("Ability on Cooldown");
            return;
        }
        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            Vector3 finalVec = new Vector3(0, 0, 0);
            finalVec += Vector3.Scale(new Vector3(-0.7f, -0.7f, -0.7f), hit.normal);
            waterObject = Instantiate(dropPrefab, hit.point + hit.normal + finalVec, Quaternion.identity);
        }
    }

    public void handleCharge(RaycastHit hit)
    {
        if (waterObject == null)
            return;

        Debug.Log(LayerMask.LayerToName(hit.transform.gameObject.layer));
        if (((hit.transform.gameObject.layer != LayerMask.NameToLayer("Water") && hit.transform.gameObject.layer != LayerMask.NameToLayer("WaterBall")) || waterObject.localScale.x > 1.5f))
        {
            CreateDrop();
            return;
        }

        waterObject.localScale = waterObject.localScale + new Vector3(0.005f, 0.005f, 0.005f);
        waterObject.gameObject.GetComponent<WaterDrop>().freezeMotion();

        Rigidbody rb = waterObject.GetComponent<Rigidbody>();
        rb.mass += 0.05f;

    }

    public void handleRelease(RaycastHit hit)
    {
        if (waterObject != null)
            CreateDrop();
    }

    private void CreateDrop()
    {
        if (waterObject != null)
        {
            waterObject.gameObject.GetComponent<WaterDrop>().freezeMotion();
            waterObject = null;
            dropCooldown = Time.time + 2f;
        }
    }
}
