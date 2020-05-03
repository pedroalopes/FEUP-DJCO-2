using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    private const float livingTime = 5.0f;
    private double damage = 1;
    private int force = 500;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
		
        if(Time.time >= startTime + livingTime) 
            evaporateDrop();
        
    }
    
    public double getForce() {
        return this.force;
    }
	public void evaporateDrop() {
        transform.localScale -= new Vector3(0.002f, 0.002f, 0.002f);
			Rigidbody rb = transform.GetComponent<Rigidbody>();
			rb.mass -= 0.01f;
		if (transform.localScale.x < 0.3)
			Destroy(gameObject);
	}
    public void printStatus() {
        Debug.Log("Collision");
        Debug.Log("Damage: " + damage);
        Debug.Log("Force: " + force);
    }
}