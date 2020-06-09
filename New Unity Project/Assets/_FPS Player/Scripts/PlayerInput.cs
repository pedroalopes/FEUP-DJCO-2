using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 input
    {
        get
        {
            Vector2 i = Vector2.zero;
            i.x = Input.GetAxis("Horizontal");
            i.y = Input.GetAxis("Vertical");
            i *= (i.x != 0.0f && i.y != 0.0f) ? .7071f : 1.0f;
            return i;
        }
    }

    public Vector2 down
    {
        get { return _down; }
    }

    public Vector2 raw
    {
        get
        {
            Vector2 i = Vector2.zero;
            i.x = Input.GetAxisRaw("Horizontal");
            i.y = Input.GetAxisRaw("Vertical");
            i *= (i.x != 0.0f && i.y != 0.0f) ? .7071f : 1.0f;
            return i;
        }
    }

    public bool run
    {
        get { return Input.GetKey(KeyCode.LeftShift); }
    }

    public bool crouch
    {
        get { return Input.GetKeyDown(KeyCode.C); }
    }

    public bool crouching
    {
        get { return Input.GetKey(KeyCode.C); }
    }

    public bool elementKeyDown { get; private set; }
    public bool elementKeyUp { get; private set; }
    public bool elementKeyPressed { get; private set; }

    public bool pickUpKeyDown { get; private set; }

    public Element currentElement { get; private set; }

    private Vector2 previous;
    private Vector2 _down;

    private int jumpTimer;
    private bool jump;

    void Start()
    {
        jumpTimer = -1;
    }

    void Update()
    {
        _down = Vector2.zero;
        if (raw.x != previous.x)
        {
            previous.x = raw.x;
            if (previous.x != 0)
                _down.x = previous.x;
        }
        if (raw.y != previous.y)
        {
            previous.y = raw.y;
            if (previous.y != 0)
                _down.y = previous.y;
        }

        chooseElement();

        elementKeyDown = Input.GetMouseButtonDown(0);
        elementKeyUp = Input.GetMouseButtonUp(0);
        elementKeyPressed = Input.GetMouseButton(0);

        pickUpKeyDown = Input.GetKeyDown(KeyCode.E);
    }

    public void FixedUpdate()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            jump = false;
            jumpTimer++;
        }
        else if (jumpTimer > 0)
            jump = true;
    }

    public bool Jump()
    {
        return jump;
    }

    public void ResetJump()
    {
        jumpTimer = -1;
    }


    private void chooseElement()
    {
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                currentElement = Element.Earth;
            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                currentElement = Element.Water;
            }
            else if (Input.GetKey(KeyCode.Alpha3))
            {
                currentElement = Element.Wind;
            }
            else if (Input.GetKey(KeyCode.Alpha4))
            {
                currentElement = Element.Fire;
            }
        }
    }
}
