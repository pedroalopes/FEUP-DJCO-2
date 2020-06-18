using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onDropCollision : MonoBehaviour
{
    public void SpawnObject()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;

        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(0.9f);
        gameObject.SetActive(false);
    }
}
