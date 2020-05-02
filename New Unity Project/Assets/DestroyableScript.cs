using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableScript : MonoBehaviour
{
    private const float racio = 1.5f;
    private float occultTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(occultTime > 0.0f) {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<MeshCollider>().enabled = false;
            StartCoroutine(ActivationRoutine());
        }
    }

    public void occultObject(float damage) {
        this.occultTime = racio * damage;
    }

    IEnumerator ActivationRoutine()
     {
         yield return new WaitForSeconds(occultTime);
         GetComponent<MeshRenderer>().enabled = true;
            GetComponent<MeshCollider>().enabled = true;
         occultTime = 0.0f;
     }

}
