using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;

public class GameData
{
    public long LastUpdated;
    public Vector3 playerPosition;
    public string currentScene;

    public GameData ()
    {
        LastUpdated = 0;
        playerPosition = Vector3.zero;
        currentScene = "Intro_Level";
    }
}
