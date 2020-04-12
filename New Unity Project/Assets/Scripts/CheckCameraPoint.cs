using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class CheckCameraPoint : MonoBehaviour {

    public Camera camera;


	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("pressed E");

            RaycastHit hit; 

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out hit)) {
                Debug.Log("Selected: " + hit.transform.name); 
                Debug.Log("Position: " + hit.point);

                /*Pintar o cubo todo*/
                hit.transform.GetComponent<MeshRenderer>().material.color = Color.black;
                

                for(var i = 0 ; i < hit.transform.GetComponent<ProBuilderMesh>().faceCount ; i++) {
                    hit.transform.GetComponent<ProBuilderMesh>().SetFaceColor(hit.transform.GetComponent<ProBuilderMesh>().faces[i], Color.black);
                }
            }
        }
	}
}
