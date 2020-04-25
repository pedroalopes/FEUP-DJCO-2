using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudObject : MonoBehaviour
{
    private const float livingTime = 10.0f;
    private double damage = 1;
    private int force = 500;
    private float startTime;
	private int rainAmount = 5;
	private float rainCooldown = 0.0f;
    public Transform waterPrefab;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
		genRain();
    }

    // Update is called once per frame
    void Update()
    {
		
        if (Time.time > rainCooldown)
			genRain();
			
        if(Time.time >= startTime + livingTime) {
            printStatus();

            Destroy(gameObject);
        }
    }
    
	private void genRain() {
		for(int i = 0; i < rainAmount; i++) {
			for(int j = 0; j < rainAmount; j++)
				Instantiate(waterPrefab, transform.position - new Vector3(-2.5f + i - Random.Range(-0.2f, 0.2f),0.5f,-2.5f + j - Random.Range(-0.2f, 0.2f)), Quaternion.identity);
		}
		rainCooldown = Time.time + 0.5f;
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