using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class DataStorageManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] bool disableDataPersistence = false;
    [SerializeField] bool initializeDataIfNull = false;
    [SerializeField] bool overrideSelectedProfileId = false;
    [SerializeField] string testSelectedProfileId = "test";

    public string firstLevelName;

    [Header("File Storage Config")]
    [SerializeField] string fileName;
    [SerializeField] bool useEncryption;

    /*[Header("Auto Saving Configuration")]
    [SerializeField] float autoSaveTimeSeconds = 60f;*/

    GameData gameData;

    List<IDataStorage> dataPersistenceObjects;

    FileDataHandler dataHandler;

    string selectedProfileId = "0";

    //Coroutine autoSaveCoroutine;

    public static DataStorageManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one!");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        if(disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence is currently disabled!");
        }

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        InitializeSelectedProfileId();
    }


    private void Start()
    {
        Debug.Log(gameData.playerPosition);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();

        Dictionary<string, GameData> profilesGameData = instance.GetAllProfilesGameData();

        GameData profileData;
        profilesGameData.TryGetValue(selectedProfileId, out profileData);

        if (SceneManager.GetActiveScene().name == firstLevelName)
        {
            Invoke("LoadGame", 0.0001f);
        }
        else
        {
            LoadGame();
        }

        /*if (autoSaveCoroutine != null)
        {
            StopCoroutine(autoSaveCoroutine);
        }
        autoSaveCoroutine = StartCoroutine(AutoSave());*/
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        this.selectedProfileId = newProfileId;

        LoadGame();
    }

    public void DeleteProfileData(string profileId)
    {
        dataHandler.Delete(profileId);

        InitializeSelectedProfileId();

        LoadGame();
    }

    void InitializeSelectedProfileId()
    {
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();

        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Overrode selected profile id with: " + testSelectedProfileId);
        }
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }
    
    public void LoadGame()
    {
       
            if (disableDataPersistence)
        {
            return;
        }
        try
        {
            this.gameData = dataHandler.Load(selectedProfileId);

        }
        catch(Exception ex) 
        {
            Debug.LogError(ex.ToString());
        }


        if (gameData != null)
        {

        }

        if(this.gameData == null && initializeDataIfNull)
        {
            Debug.Log("There is no data here");
            NewGame();
        }

        if (this.gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded!");
            return;
        }

        foreach (IDataStorage dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (disableDataPersistence)
        {
            return;
        }

        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved!");
            return;
        }

        foreach(IDataStorage dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }

        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        dataHandler.Save(gameData, selectedProfileId);
    }

    //private void OnApplicationQuit()
    //{
    //    SaveGame();
    //}

    List<IDataStorage> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataStorage> dataPersistenceObjects = 
            FindObjectsOfType<MonoBehaviour>().OfType<IDataStorage>();

        return new List<IDataStorage>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    /*IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveTimeSeconds);
            SaveGame();
            Debug.Log("Auto Save Complete");
        }
    }*/
}
