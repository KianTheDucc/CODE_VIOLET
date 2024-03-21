using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataStorage
{
    void LoadData(GameData data);

    void SaveData(GameData data);   
}
