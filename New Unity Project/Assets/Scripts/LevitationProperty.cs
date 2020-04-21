using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitationProperty : MonoBehaviour
{
    [SerializeField] private float heightLevel = 0f;
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


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
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

    public void EnableLevitate()
    {
        isLevitating = true;
        incrementHeight(height_increment);
        effect_countdown = 0;
    }

    private void incrementHeight(float inc)
    {
        heightLevel += inc;

        if (heightLevel >= 10)
        {
            heightLevel = 10;
        }
    }

    private void StopLevitating()
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
