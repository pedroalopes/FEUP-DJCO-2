using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterObject : MonoBehaviour
{
    private const float livingTime = 10.0f;
    private double damage = 0.1;
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
        if(Time.time >= startTime + livingTime) {
            Destroy(gameObject);
        }
    }
	

    public void addDamage() {
        damage += 0.1;
    }
	
    public void removeForce() {
        force -= 6;
    }
    
    void OnCollisionEnter(Collision collision)
    {
    }

    public double getForce() {
        return this.force;
    }

}