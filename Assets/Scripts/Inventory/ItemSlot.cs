// Credit to https://www.youtube.com/@NightRunStudio for his Inventory System Tutorial series.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    //======ITEM DATA======//
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;

    [SerializeField]
    private int maxNumberOfItems;  // Defines the size of the slot, allows you to stack them


    //======ITEM SLOT======//
    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image itemImage;

    //======ITEM DESCRIPTION======//
    public Image itemDescriptionImage;
    public TMP_Text ID_NameText;
    public TMP_Text ID_Text;

    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        // CHeck to see if the slot is already full
        if (isFull)
            return quantity;

        //Update Name
        this.itemName = itemName;

        // Update Image
        this.itemSprite = itemSprite;
        itemImage.sprite = itemSprite;
        itemImage.enabled = true;

        // Update Description
        this.itemDescription = itemDescription;

        // Update Quantity

        if (this.quantity == 0) 
        {
            this.quantity = 1;
        }
        else
        {
            this.quantity += quantity;
        }

        if (this.quantity >= maxNumberOfItems)
        {
            quantityText.text = maxNumberOfItems.ToString();
            quantityText.enabled = true;
            isFull = true;

            // Return leftover items
            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;
            return extraItems;
        }

        // Update Quantity Text
        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;

        return 0;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
                
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
    {
        inventoryManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;
        ID_NameText.text = itemName;
        ID_Text.text = itemDescription;
        itemDescriptionImage.sprite = itemSprite;
    }

    public void OnRightClick()
    {

    }
}
