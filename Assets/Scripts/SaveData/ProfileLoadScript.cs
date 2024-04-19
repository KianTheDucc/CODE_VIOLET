using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileLoadScript : MonoBehaviour
{

    [SerializeField]
    private string SelectedProfileID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadOrNewGame()
    {
        DataStorageManager.instance.SelectedProfileID = SelectedProfileID;
        if (!DataStorageManager.instance.HasGameData())
        {
            DataStorageManager.instance.NewGame();
        }
        else
        {
            DataStorageManager.instance.LoadGame();
        }
    }
}
