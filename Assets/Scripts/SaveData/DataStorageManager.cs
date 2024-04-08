using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataStorageManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData GameData;

    private List<IDataStorage> DataStorageList;

    private FileDataHandler dataHandler;
    
    private string SelectedProfileID = "";



    public static DataStorageManager instance { get; private set; }


    private void Awake()
    {
        if  (instance != null)
        {
            Debug.Log("More than one DataStorageManager found in scene, deleting newest one");
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        DontDestroyOnLoad(gameObject);
    }


    private void OnEnable()
    {
           SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;   
    }

    public void  OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.DataStorageList = FindAllDataStorageObjects();
        LoadGame();
    }

    public void ChangeSelectedProfileID(string newProfileID)
    {
        this.SelectedProfileID = newProfileID;
        LoadGame();
    }

    public void DeleteProfileData(string profileId)
    {
        //Delete the data for this profile id
        dataHandler.Delete(profileId);
        //initalize t he selected profile id
        InitializeSelectedProfileId();
        //reload the game so that the data matches the newly selected profileId
        LoadGame();
    }

    private void InitializeSelectedProfileId()
    {
        this.SelectedProfileID = dataHandler.GetMostRecentlyUpdatedProfileId();
    }

    public void NewGame()
    {
        this.GameData = new GameData();
    }

    public void LoadGame()
    {
        this.GameData = dataHandler.Load(SelectedProfileID);

        if (this.GameData == null)
        {
            NewGame();
            Debug.Log("New game initialised");
        }

        foreach (IDataStorage storage in DataStorageList)
        {
            storage.LoadData(GameData);
        }
    }

    public void SaveGame()
    {
        if (this.GameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved");
        }

        // pass the data to other scripts so they can  update it

        foreach (IDataStorage storage in DataStorageList)
        {
            storage.SaveData(GameData);
        }

        // save the timestamp of now in b inary so we know when it was last saved
        GameData.LastUpdated = System.DateTime.Now.ToBinary();

        dataHandler.Save(GameData, SelectedProfileID);
    }

    private List<IDataStorage> FindAllDataStorageObjects()
    {
        IEnumerable<IDataStorage> dataStorageObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataStorage>();

        return new List<IDataStorage>(dataStorageObjects);
    }

    public bool HasGameData()
    {
        return GameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }


}
