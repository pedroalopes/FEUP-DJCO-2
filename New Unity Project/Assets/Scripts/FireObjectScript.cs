using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObjectScript : MonoBehaviour
{
    private const float livingTime = 10.0f;
    private float damage;
    private int force;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        damage = 1.0f;
        force = 600;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= startTime + livingTime) {
            printStatus();

            Destroy(gameObject);
        }
    }

    public void addDamage() {
        damage += 0.1f;
    }

    public void removeForce() {
        force -= 6;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        printStatus();

		GameObject gameObjectCollision = collision.gameObject;

		if(gameObjectCollision.tag == "Destroyable") {
            gameObjectCollision.GetComponent<DestroyableScript>().occultObject(damage);
        }

        Destroy(gameObject);
    }

    public double getForce() {
        return this.force;
    }

    public void printStatus() {
        Debug.Log("Collision");
        Debug.Log("Damage: " + damage);
        Debug.Log("Force: " + force);
    }
}