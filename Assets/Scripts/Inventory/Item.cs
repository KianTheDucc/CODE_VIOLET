// Credit to https://www.youtube.com/@NightRunStudio for his Inventory System Tutorial series.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private string itemName;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private Sprite sprite;

    [TextArea]
    [SerializeField]
    private string itemDescription;

    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            /*int leftOverItems = */
            inventoryManager.AddItem(itemName, quantity, transform.GetComponent<SpriteRenderer>().sprite, itemDescription);

            Destroy(this.gameObject);
            //if (leftOverItems <= 0)
            //{

            //}
            //else
            //{
            //    quantity = leftOverItems;
            //}
        }
    }


}