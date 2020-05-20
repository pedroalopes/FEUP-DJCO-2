using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterButton : MonoBehaviour
{
    [SerializeField]
    GameObject door;

    private const float ratio = 1f;
    private float openTime = 0.0f;
	
    bool isOpened = false;
    

    private void Update()
    {
        if(isOpened && Time.time >= openTime)
        {
			closeDoor();
        }

    }
    public void OpenDoor(float damage) {
		if(!isOpened){
			this.openTime = Time.time + ratio * damage;
			door.gameObject.transform.GetChild(0).position -= new Vector3(0, 6, 0);
			door.gameObject.transform.GetChild(1).position -= new Vector3(0, 6, 0);
			isOpened = true;
		}
	}
    private void closeDoor() {
		if(isOpened){
			door.gameObject.transform.GetChild(0).position += new Vector3(0, 6, 0);
			door.gameObject.transform.GetChild(1).position += new Vector3(0, 6, 0);
			isOpened = false;
		}
	}
	
}
