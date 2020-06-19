using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Controls
{
    public KeyCode moveForward;
    public KeyCode moveBack;
    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode catchObject;
    public KeyCode levitateObject;
    public KeyCode crouch;
    public KeyCode jump;
    public KeyCode run;
    public KeyCode fire;
    
    public Controls()
    {
        this.moveForward = KeyCode.W;
        this.moveBack = KeyCode.S;
        this.moveLeft = KeyCode.A;
        this.moveRight = KeyCode.D;
        this.catchObject = KeyCode.E;
        this.levitateObject = KeyCode.E;
        this.crouch = KeyCode.C;
        this.jump = KeyCode.Space;
        this.run = KeyCode.LeftShift;
        this.fire = KeyCode.Mouse0;
    }

    public KeyCode getControl(string name) {
        if(name == "moveForward") {
            return moveForward;
        } else if(name == "moveBack") {
            return moveBack;
        } else if(name == "moveLeft") {
            return moveLeft;
        } else if(name == "moveRight") {
            return moveRight;
        } else if(name == "catchObject") {
            return catchObject;
        } else if(name == "levitateObject") {
            return levitateObject;      
        } else if(name == "fire") {
            return fire;
        } else if(name == "crouch") {
            return crouch;
        } else if(name == "jump") {
            return jump;
        } else if(name == "run") {
            return run;
        }
        return KeyCode.Z;
    }
    
    public void setControl(string name, KeyCode keyCode) {
        if(name == "moveForward") {
            moveForward = keyCode;
        } else if(name == "moveBack") {
            moveBack = keyCode;
        } else if(name == "moveLeft") {
            moveLeft = keyCode;
        } else if(name == "moveRight") {
            moveRight = keyCode;
        } else if(name == "catchObject") {
            catchObject = keyCode;
        } else if(name == "levitateObject") {
            levitateObject = keyCode;      
        } else if(name == "fire") {
            fire = keyCode;
        } else if(name == "crouch") {
            crouch = keyCode;
        } else if(name == "jump") {
            jump = keyCode;
        } else if(name == "run") {
            run = keyCode;
        }
    }
}