using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }
    public List<Item> ownedItems = new List<Item>();

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("PlayerInventory instance created.");
        }
        else
        {
            Debug.Log("Duplicate PlayerInventory found. Destroying this instance: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    public void ResetInventory()
    {
        ownedItems.Clear();
        Debug.Log("Player inventory has been reset. Current item count: " + ownedItems.Count);
    }

    public void AddItem(Item item)
    {
        if (!ownedItems.Contains(item))
        {
            ownedItems.Add(item);
            Debug.Log($"Added {item.itemName} to inventory. New Count: {ownedItems.Count}");
        }
        else
        {
            Debug.Log($"{item.itemName} is already in inventory.");
        }
    }

    public bool HasItem(string itemName)
    {
        foreach (var item in ownedItems)
        {
            Debug.Log($"Checking item: {item.itemName}"); // Log each item
            if (item.itemName == itemName)
            {
                return true;
            }
        }
        return false;
    }
}
