using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseItemSpawner : MonoBehaviour
{
    public Transform spawnPoint; // Assign the spawn point in the inspector
    public Transform pistolSpawnPoint; // New spawn point for pistol items

    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = PlayerInventory.Instance; // Get the singleton instance

        // Spawn all owned items in the house
        foreach (Item item in playerInventory.ownedItems)
        {
            SpawnItem(item);
        }
    }

    private void SpawnItem(Item item)
    {
        if (item.prefab != null)
        {
            // Check if the item's tag is "Pistol"
            Transform chosenSpawnPoint = item.prefab.CompareTag("Pistol") ? pistolSpawnPoint : spawnPoint;

            // Instantiate at the correct spawn point
            GameObject spawnedItem = Instantiate(item.prefab, chosenSpawnPoint.position, Quaternion.identity);
            spawnedItem.name = item.itemName; // Set the name for clarity
            Debug.Log($"Spawned {item.itemName} at {chosenSpawnPoint.position}");
        }
        else
        {
            Debug.LogWarning($"Item {item.itemName} has no prefab assigned!");
        }
    }
}