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
        playerPosition = new Vector3(377.1f, -135.6f, 0);
        currentScene = "Intro_Level";
    }
}
