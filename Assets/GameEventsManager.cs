using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEventsManager : MonoBehaviour, IDataStorage
{
    public static GameEventsManager instance { get; private set; }

    private PlayerController playerController;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one GameEventsManager in the scene, destroying newest one");
            Destroy(this);
            return;
        }
        instance = this;

        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene oldScene, Scene newScene)
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void LoadData(GameData gameData)
    {
        if (gameData.currentScene == SceneManager.GetActiveScene().name)
        {
            playerController.transform.position = gameData.playerPosition;
        }

    }

    public void SaveData(GameData gameData)
    {
        gameData.currentScene = SceneManager.GetActiveScene().name;

        gameData.playerPosition = playerController.transform.position;
    }
}