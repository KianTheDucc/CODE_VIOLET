// Credit to https://www.youtube.com/@NightRunStudio for his Inventory System Tutorial series.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour, IDataStorage
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlot;

    public List<string> keyList = new List<string>();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I) && menuActivated)
        {
            InventoryMenu.SetActive(false);
            menuActivated = false;
        }

        else if (Input.GetKeyDown(KeyCode.I) && !menuActivated)
        {
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }
    }

    public void LoadData(GameData gameData)
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            foreach (var key in gameData.keys)
            {
                if (!keyList.Contains(key))
                {
                    keyList.Add(key);
                }
                print("keyadded1");
            }
        }

    }
    public void SaveData(GameData gameData)
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {


            foreach (var key in keyList)
            {
                if (!gameData.keys.Contains(key))
                {
                    gameData.keys.Add(key);
                }
                print("keyadded2");
            }
        }
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (!itemSlot[i].isFull && itemSlot[i].name == name || itemSlot[i].quantity == 0) 
            {
                int leftOverItems = itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription);
                if (leftOverItems > 0)
                {
                    leftOverItems = itemSlot[i].AddItem(itemName, leftOverItems, itemSprite, itemDescription);
                }
                return leftOverItems;
            }
        }
        return quantity;
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }
}
