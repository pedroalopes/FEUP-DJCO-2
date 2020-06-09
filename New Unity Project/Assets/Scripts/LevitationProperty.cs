using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitationProperty : MonoBehaviour
{
    [SerializeField] private float heightLevel;
    [SerializeField] private float startHeight;
    [SerializeField] private float maxLevitateHeight;
    [SerializeField] private float levitateHeight = 1f;
    [SerializeField] private float bounceDamp = 0.05f;
    [SerializeField] private Vector3 centreOffset;
    [SerializeField] private float effect_duration = 5f;
    [SerializeField] private float effect_countdown = 0f;
    [SerializeField] private float height_increment = 0.1f;

    private float forceFactor;
    private Vector3 actionPoint;
    private Vector3 upLift;
    private Rigidbody rb;

    private bool isLevitating = false;
    private bool levitate = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        maxLevitateHeight = 5;
    }

    void Update()
    {
        if (isLevitating)
        {
            Levitate();
            effect_countdown += Time.deltaTime;

            if (effect_countdown >= effect_duration)
            {
                StopLevitating();
                effect_countdown = 0;
            }
        }
    }

    public float getHeightLevel()
    {
        return heightLevel;
    }

    public void EnableLevitate(float aimingAtY)
    {
        isLevitating = true;
        setHeightLevel(aimingAtY);
        effect_countdown = 0;
    }

    private void setHeightLevel(float level)
    {
        Debug.DrawRay(transform.position + transform.TransformDirection(centreOffset), Vector3.down, Color.red);

        RaycastHit[] hits = Physics.RaycastAll(transform.position + transform.TransformDirection(centreOffset), Vector3.down, maxLevitateHeight);

        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log(LayerMask.LayerToName(hits[i].transform.gameObject.layer));
            if (hits[i].transform.gameObject.layer != LayerMask.NameToLayer("MoveableObject") && hits[i].transform.gameObject.layer != LayerMask.NameToLayer("LevitateObject"))
            {
                heightLevel = level;
                return;
            }
        }
    }

    private void incrementHeight(float inc)
    {
        heightLevel += inc;

        if (heightLevel >= 10)
        {
            heightLevel = 10;
        }
    }

    public void StopLevitating()
    {
        isLevitating = false;
        heightLevel = 0;
    }

    void Levitate()
    {

        actionPoint = transform.position + transform.TransformDirection(centreOffset);
        forceFactor = 1f - ((actionPoint.y - heightLevel) / levitateHeight);

        if (forceFactor > 0f)
        {
            upLift = -Physics.gravity * (forceFactor - rb.velocity.y * bounceDamp);
            rb.AddForceAtPosition(upLift, actionPoint);
        }

    }
}
