[System.Serializable]
public class Display
{
    public float ambientLight;
    
    public float getDisplay(string name) {
        if(name == "ambientLight") {
            return ambientLight;
        }
        return 0.0f;
    }
    
    public void setDisplay(string name, float value) {
        if(name == "ambientLight") {
            ambientLight = value;
        }
    }
}