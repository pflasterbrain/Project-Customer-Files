using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellItems : MonoBehaviour
{
    public PlayerController playerInteract;
    public GameManager gameManager;
    public List<Item> itemsForSale;
    public Pickup pickupScript;  // Reference to the Pickup script to access the held item
    
    private void Start()
    {
        SetItemSellPrices();
    }
    
    public void SetItemSellPrices()
    {
        foreach (Item item in itemsForSale)
        {
            Debug.Log($"{item.itemName} is being sold for {item.sellprice} gold.");
        }
    }

    void Update()
    {
        // Check if the player can put down an item and is holding something
        if (playerInteract.canSell && pickupScript.hasItem)
        {
            if (Input.GetKeyDown("e"))
            {
                // Directly access heldItemStats
                if (pickupScript.heldItemStats != null)
                {
                    SellItem(pickupScript.heldItemStats);
                }
                else
                {
                    Debug.LogWarning("No item stats available for the held item!");
                }
            }
        }
    }

    private void SellItem(Item item)
    {
        // Reference to the item currently being held
        GameObject heldItem = pickupScript.heldItem;

        if (heldItem != null)
        {
            // Re-enable physics and collider on the object
            Destroy(heldItem);
            GameManager.Instance.AddMoney(item.sellprice);

            // Clear the reference to the held item
            pickupScript.hasItem = false;
            pickupScript.heldItem = null;
            pickupScript.heldItemStats = null; // Clear the held item stats

            Debug.Log($"Sold item: {item.itemName} for {item.sellprice} gold.");
        }
    }
}
