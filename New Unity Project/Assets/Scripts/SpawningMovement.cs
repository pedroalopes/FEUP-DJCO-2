using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningMovement : MonoBehaviour
{
    public float speed = 2.5f;
    private Vector3 initialPos;
    public Vector3 normalDir;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, initialPos + Vector3.Scale(new Vector3(2,2,2),normalDir),step);

    }
}
