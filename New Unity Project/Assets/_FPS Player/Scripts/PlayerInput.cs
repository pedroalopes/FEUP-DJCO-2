using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ManageUserSettings;

public class PlayerInput : MonoBehaviour
{
    private UserSettings userSettings;


    public Vector2 input
    {
        get
        {
            Vector2 i = Vector2.zero;

            if (Input.GetKey(userSettings.controls.moveRight))
            {
                i.x += 1;
            }
            if (Input.GetKey(userSettings.controls.moveLeft))
            {
                i.x -= 1;
            }
            if (Input.GetKey(userSettings.controls.moveForward))
            {
                i.y += 1;
            }
            if (Input.GetKey(userSettings.controls.moveBack))
            {
                i.y -= 1;
            }

            //i.x = Input.GetAxis("Horizontal");
            //i.y = Input.GetAxis("Vertical");
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

            if (Input.GetKey(userSettings.controls.moveRight))
            {
                i.x += 1;
            }
            if (Input.GetKey(userSettings.controls.moveLeft))
            {
                i.x -= 1;
            }
            if (Input.GetKey(userSettings.controls.moveForward))
            {
                i.y += 1;
            }
            if (Input.GetKey(userSettings.controls.moveBack))
            {
                i.y -= 1;
            }

            i *= (i.x != 0.0f && i.y != 0.0f) ? .7071f : 1.0f;
            return i;
        }
    }

    public bool run
    {
        get { return Input.GetKey(userSettings.controls.run); }
    }

    public bool crouch
    {
        get { return Input.GetKeyDown(userSettings.controls.crouch); }
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
        userSettings = ManageUserSettings.LoadUserSettings();
    }

    void Update()
    {
        userSettings = ManageUserSettings.LoadUserSettings();
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

        pickUpKeyDown = Input.GetKeyDown(userSettings.controls.catchObject);

        userSettings = ManageUserSettings.LoadUserSettings();
    }

    public void FixedUpdate()
    {
        if (!Input.GetKey(userSettings.controls.jump))
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
