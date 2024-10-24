using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ItemSelection : MonoBehaviour
{
    private int currentItem = 0;

    public List<Item> items; // Holds Item objects
    private bool[] itemBought;

    public TMP_Text priceTagText;
    public TMP_Text itemNameText;
    public Button buyButton;

    private bool isInitialized = false; // Flag to check if initialization is done

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        // Wait until GameManager is ready
        while (GameManager.Instance == null)
        {
            yield return null; // Wait until next frame
        }

        Debug.Log("GameManager is now available.");

        // Ensure that items are populated and all references are set correctly
        if (items == null || items.Count == 0)
        {
            Debug.LogError("Items list is not set or empty! Ensure items are populated.");
            yield break;
        }

        if (priceTagText == null || itemNameText == null || buyButton == null)
        {
            Debug.LogError("One or more UI references are not set in the Inspector.");
            yield break;
        }

        // Initialize itemBought array with same length as items
        itemBought = new bool[items.Count];
        for (int i = 0; i < itemBought.Length; i++)
        {
            itemBought[i] = false;
        }

        isInitialized = true;

        // Select the initial item
        SelectItem(currentItem);

        // Initial money display update
        GameManager.Instance.UpdateMoneyDisplay();
    }

    private void SelectItem(int _index)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("ItemSelection is not yet initialized.");
            return;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == _index);
        }

        UpdatePriceTag(_index);
        UpdateItemName(_index);
        UpdateBuyButtonState();
    }

    public void ChangeItem(int _change)
    {
        if (!isInitialized) return;

        currentItem += _change;

        if (currentItem < 0)
        {
            currentItem = transform.childCount - 1;
        }
        else if (currentItem >= transform.childCount)
        {
            currentItem = 0;
        }

        SelectItem(currentItem);
    }

    private void UpdatePriceTag(int itemIndex)
    {
        if (items != null && itemIndex >= 0 && itemIndex < items.Count)
        {
            priceTagText.text = "$" + items[itemIndex].price.ToString("F2");
        }
        else
        {
            priceTagText.text = "Price not available";
        }
    }

    private void UpdateItemName(int itemIndex)
    {
        if (items != null && itemIndex >= 0 && itemIndex < items.Count)
        {
            itemNameText.text = items[itemIndex].itemName;
        }
        else
        {
            itemNameText.text = "Item name not available";
        }
    }

    private void UpdateBuyButtonState()
    {
        if (!isInitialized || items == null || currentItem < 0 || currentItem >= items.Count)
        {
            buyButton.interactable = false;
            return;
        }

        if (itemBought[currentItem] || GameManager.Instance.playerMoney < items[currentItem].price)
        {
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
    }

    public void BuyItem()
    {
        if (!isInitialized) return;

        Debug.Log($"Attempting to buy item: {items[currentItem].itemName}, Price: {items[currentItem].price}, Player Money: {GameManager.Instance.playerMoney}");

        if (GameManager.Instance.playerMoney >= items[currentItem].price)
        {
            GameManager.Instance.SubtractMoney((int)items[currentItem].price);
            itemBought[currentItem] = true;

            // Add item to the player inventory
            PlayerInventory.Instance.AddItem(items[currentItem]);

            UpdateBuyButtonState();
            Debug.Log($"{items[currentItem].itemName} purchased successfully!");
        }
        else
        {
            Debug.Log("Not enough money to purchase the item.");
        }
        Debug.Log("Current Inventory:");
        foreach (var item in PlayerInventory.Instance.ownedItems)
        {
            Debug.Log($"- {item.itemName}");
        }
    }

}
