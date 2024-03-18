using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class spawnHere : MonoBehaviour
{
    public GameObject Player;
    public GameObject Spawn;
    public SceneLoadedUnityEvent myEvent;
    // Start is called before the first frame update
    void Start()
    {
        //Player = GameObject.Find("Player");
        //Spawn = GameObject.Find(PlayerPrefs.GetString("SpawnLocation"));
        //if (Spawn != null)
        //{
        //    Player.transform.position = Spawn.transform.position;
        //}
    }
    public void onSceneChanged()
    {
        Player = GameObject.Find("Player");
        Spawn = GameObject.Find(PlayerPrefs.GetString("SpawnLocation"));
        if (Spawn != null)
        {
            Player.transform.position = Spawn.transform.position;
        }
    }
    

    // Update is called once per frame
    void Update()
    {

    }

}
