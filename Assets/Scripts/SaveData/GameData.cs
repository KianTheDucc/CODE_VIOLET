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
        playerPosition = new Vector3(539.43f, -294.57f, 0);
        currentScene = "Core_Level";
    }
}
