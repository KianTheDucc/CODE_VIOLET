using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEventsManager : MonoBehaviour, IDataStorage
{
    public static GameEventsManager instance { get; private set; }

    private PlayerController playerController;

    private InventoryManager inventoryManager;

    public List<string> keysCollected;

    public Scene oldscene;
    private Scene newscene;
    private Scene FirstScene;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one GameEventsManager in the scene, destroying newest one");
            Destroy(this);
            return;
        }
        else
        {
            instance = this;

            DontDestroyOnLoad(this);
        }
        FirstScene = SceneManager.GetSceneByBuildIndex(0);
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void OnSceneLoaded(Scene oldScene, Scene newScene)
    {
        print("sceneloaded");
        if (newScene.buildIndex != 0)
        {
            playerController = FindObjectOfType<PlayerController>();
            inventoryManager = FindObjectOfType<InventoryManager>();
            inventoryManager.keyList = keysCollected;   
        }
        oldscene = oldScene;
        Time.timeScale = 1f;
    }

    void OnSceneUnloaded(Scene current)
    {
        if (inventoryManager != null)
        {
            keysCollected = inventoryManager.keyList;
        }

        Time.timeScale = 1f;
    }

    public void LoadData(GameData gameData)
    {
        if (oldscene == FirstScene && SceneManager.GetActiveScene().buildIndex != 0 && playerController != null && SceneManager.GetActiveScene().name == gameData.currentScene)
        {
            playerController.transform.position = gameData.playerPosition;
        }

    }

    public void SaveData(GameData gameData)
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            gameData.currentScene = SceneManager.GetActiveScene().name;
            if (playerController != null)
            {
                gameData.playerPosition = playerController.transform.position;
            }
        }



    }
}
