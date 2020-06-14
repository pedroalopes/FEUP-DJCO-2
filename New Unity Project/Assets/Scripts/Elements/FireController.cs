using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ManageUserSettings;

public class FireController : MonoBehaviour
{
	
	[FMODUnity.EventRef]
    public string FireEvent = "";
	FMOD.Studio.EventInstance fire;

    [FMODUnity.EventRef]
    public string FireShootEvent = "";
    FMOD.Studio.EventInstance fireShoot;
	
    public Transform firePrefab;
    public GameObject firePoint;
    private Transform fireObject;
	private bool soundEnabled;
	
    private UserSettings userSettings;

    public void handleStart()
    {
        userSettings = ManageUserSettings.LoadUserSettings();
        soundEnabled = userSettings.sound.getSound("playerSounds");
		
		
		fire = FMODUnity.RuntimeManager.CreateInstance(FireEvent);
        fire.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
		playFireStart();
		
		fireShoot = FMODUnity.RuntimeManager.CreateInstance(FireShootEvent);
		
        if (fireObject == null)
        {
			//fireShoot = FMODUnity.RuntimeManager.CreateInstance(FireShootEvent);
            fireObject = Instantiate(firePrefab, Camera.main.transform.position + (Camera.main.transform.forward * 1), transform.rotation);
            fireObject.SetParent(firePoint.transform);
        }
    }
    public void handleCharge()
    {
        fireShoot.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
        if (fireObject == null)
            return;

        if (fireObject.localScale.x < 1)
        {
            fireObject.localScale = fireObject.localScale + new Vector3(0.01f, 0.01f, 0.01f);
            fireObject.GetComponent<FireObjectScript>().addDamage();
            fireObject.GetComponent<FireObjectScript>().removeForce();
        }

    }
	void Update () {
		
		fire.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
		
        //update sound status
        userSettings = ManageUserSettings.LoadUserSettings();
        soundEnabled = userSettings.sound.getSound("playerSounds");
	}

    public void handleRelease(RaycastHit hit)
    {
        if (fireObject == null)
            return;

        Rigidbody rb = fireObject.gameObject.GetComponent<Rigidbody>();
        Vector3 shoot = (hit.point - fireObject.position).normalized;

        rb.AddForce(shoot * (int)fireObject.GetComponent<FireObjectScript>().getForce());
        fireObject.SetParent(null);
        fireObject = null;
		playFireShoot();
    }
	void playFireStart() {
		if(soundEnabled)
			fire.start();
	}
	void playFireShoot() {
		if(soundEnabled)
			fireShoot.start();
	}
}
