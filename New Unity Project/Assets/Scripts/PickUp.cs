using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    Vector3 objectPos;
    public bool canHold = true;
    public GameObject item;
    public GameObject tempParent;
    public bool isBeingHeld = false;
    public float pickUpDistance = 0.5f;
    private Vector3 initialPosition;
    private LevitationProperty levitationProperty;



    private void Start()
    {
        initialPosition = transform.position;
        item.GetComponent<Rigidbody>().detectCollisions = true;
        levitationProperty = this.GetComponentInParent<LevitationProperty>();

    }

    private void Update()
    {
        if (isBeingHeld)
        {
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }


    public bool Interact()
    {
        if (!isBeingHeld)
            PickUpObject();
        else
            Drop();

        return isBeingHeld;

    }

    private void PickUpObject()
    {
        item.GetComponent<Rigidbody>().useGravity = false;
        item.GetComponent<Rigidbody>().velocity = Vector3.zero;
        item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        item.transform.SetParent(tempParent.transform);
        item.GetComponent<LevitationProperty>().StopLevitating();
        isBeingHeld = true;
    }

    public void Drop()
    {
        item.GetComponent<Rigidbody>().useGravity = true;
        item.transform.SetParent(null);
        isBeingHeld = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("DestroyCube"))
        {
            gameObject.transform.position = initialPosition;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
            return;

        levitationProperty.StopLevitating();
    }
}
