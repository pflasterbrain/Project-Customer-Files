using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public List<Item> ownedItems = new List<Item>(); // List to hold owned ScriptableObject items

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object between scene loads
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate PlayerInventory
        }
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

}
