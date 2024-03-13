// Credit to https://www.youtube.com/@NightRunStudio for his Inventory System Tutorial series.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
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
