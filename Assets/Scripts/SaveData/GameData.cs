using UnityEngine;

[System.Serializable]
public class GameData
{
    public long LastUpdated;
    public Vector3 playerPosition;
    public string currentScene;
    
    public long lastUpdated;

    public GameData ()
    {
        playerPosition = new Vector3(358.7f, -192.4f, 0);
        currentScene = "Intro_Level";
    }
}
