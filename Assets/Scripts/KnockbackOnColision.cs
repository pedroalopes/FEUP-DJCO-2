using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackOnColision : MonoBehaviour
{
    [SerializeField] private float knockbackStrength;

    private void OnCollisionEnter(Collision other) {
        Rigidbody rb = other.collider.GetComponent<Rigidbody>();

        if (rb != null) {
            Vector3 direction = other.transform.position - transform.position;
            
            // No vertical knockback
            direction.y = 0;
            
            rb.AddForce(direction.normalized * knockbackStrength, ForceMode.Impulse);
        }    
    }
}
