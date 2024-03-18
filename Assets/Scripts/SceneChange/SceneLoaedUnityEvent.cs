using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneLoadedUnityEvent : UnityEvent<GameObject>
{
    public GameObject Player;
    public GameObject Spawn;
    
    public SceneLoadedUnityEvent()
    {
        Player = GameObject.Find(PlayerPrefs.GetString("Player"));
        Spawn = GameObject.Find(PlayerPrefs.GetString("SpawnLocation"));
    }

    public void OnSceneChanged()
    {
        Player.transform.position = Spawn.transform.position;
    }
}
