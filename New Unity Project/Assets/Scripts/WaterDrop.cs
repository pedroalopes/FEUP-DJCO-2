using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    private const float livingTime = 10.0f;
    private double damage = 1;
    private int force = 500;
    private float startTime;
    public Transform waterPrefab;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
		
        if(Time.time >= startTime + livingTime) 
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