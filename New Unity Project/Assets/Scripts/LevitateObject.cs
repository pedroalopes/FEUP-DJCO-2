using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitateObject : MonoBehaviour
{
    [SerializeField] private float heightLevel = 4f;
    [SerializeField] private float levitateHeight = 1f;
    [SerializeField] private float bounceDamp = 0.05f;
    [SerializeField] private Vector3 centreOffset;

    [SerializeField] private float forceFactor;
    [SerializeField] private Vector3 actionPoint;
    [SerializeField] private Vector3 upLift;
    private Rigidbody selection_rb;
    private Transform selection_transform;

    private void Start()
    {
    }

    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetKey(KeyCode.B))
        {
            if (Physics.Raycast(ray, out hit))
            {

                if (!hit.transform.CompareTag("Levitate"))
                {
                    Debug.Log("Can't levitate that");
                    return;
                }

                LevitationProperty prop = hit.collider.gameObject.GetComponent<LevitationProperty>();
                prop.EnableLevitate();
            }
        }
    }
}
