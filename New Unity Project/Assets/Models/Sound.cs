[System.Serializable]
public class Sound
{
    public bool ambientMusic;
    public bool playerSounds;
    
    public bool getSound(string name) {
        if(name == "ambientMusic") {
            return ambientMusic;
        } else if(name == "playerSounds") {
            return playerSounds;
        }
        return false;
    }
    
    public void setSound(string name, bool status) {
        if(name == "ambientMusic") {
            ambientMusic = status;
        } else if(name == "playerSounds") {
            playerSounds = status;
        }
     }
}