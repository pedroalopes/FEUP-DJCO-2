using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class ManageUserSettings : MonoBehaviour
{
    private static string path = "Assets/userSettings.json";

    public static UserSettings LoadUserSettings()
    {
        string jsonString = File.ReadAllText(path);
        UserSettings userSettings = JsonUtility.FromJson<UserSettings>(jsonString);
        return userSettings;
    }

    public static void SaveUserSettings(UserSettings userSettings)
    {
        string jsonString = JsonUtility.ToJson(userSettings);
        File.WriteAllText(path, jsonString);
    }
}
