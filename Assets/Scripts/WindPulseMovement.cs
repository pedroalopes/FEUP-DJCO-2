using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPulseMovement : MonoBehaviour
{
    public float speed;
    public float fireRate;

    private void Start() {
        
    }

    private void Update() {
        if(speed != 0) {
            transform.position += transform.forward * (speed * Time.deltaTime);
        } else {
            Debug.Log("No Speed");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(this.gameObject);
    }
}
