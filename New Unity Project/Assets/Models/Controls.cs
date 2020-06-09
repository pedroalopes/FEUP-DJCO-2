using UnityEngine;

[System.Serializable]
public class Controls
{
    public string moveForward;
    public string moveBack;
    public string moveLeft;
    public string moveRight;
    public string catchObject;
    public string levitateObject;
    public string fire;

    public string getControl(string name) {
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
        }
        return "";
    }
    
    public void setControl(string name, string text) {
        if(name == "moveForward") {
            moveForward = text.ToUpper();
        } else if(name == "moveBack") {
            moveBack = text.ToUpper();
        } else if(name == "moveLeft") {
            moveLeft = text.ToUpper();
        } else if(name == "moveRight") {
            moveRight = text.ToUpper();
        } else if(name == "catchObject") {
            catchObject = text.ToUpper();
        } else if(name == "levitateObject") {
            levitateObject = text.ToUpper();      
        } else if(name == "fire") {
            fire = text.ToUpper();
        }
    }
}