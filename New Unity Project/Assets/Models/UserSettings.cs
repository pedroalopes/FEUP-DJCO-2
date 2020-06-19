using UnityEngine;

[System.Serializable]
public class UserSettings
{
	public Level level;
    public Sound sound;
    public Display display;
    public Controls controls;

    public UserSettings()
    {
	    this.level = new Level();
	    this.sound = new Sound();
	    this.display = new Display();
	    this.controls = new Controls();
    }

}