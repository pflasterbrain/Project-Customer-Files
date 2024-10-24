using UnityEngine;

public class HouseItemSpawner : MonoBehaviour
{
    public Transform spawnPoint; // Assign the default spawn point in the inspector
    public Transform pistolSpawnPoint; // Assign the pistol-specific spawn point
    public Transform holsterSpawnPoint; // Assign the holster-specific spawn point
    public GameManager gameManager;
    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = PlayerInventory.Instance; // Get the singleton instance

        // Debugging current owned items
        Debug.Log("Current items in inventory: " + playerInventory.ownedItems.Count);

        // Spawn all owned items in the house
        foreach (Item item in playerInventory.ownedItems)
        {
            Debug.Log($"Attempting to spawn item: {item.itemName}");
            SpawnItem(item);
        }
    }

    // This method checks if the item should be spawned
    private bool ShouldSpawnItem(Item item)
    {
        // Example: Check game state, whether it is a fresh start, or the item should be reset.
        // You can add any additional conditions based on your game logic here.
        if (item.itemName == "Gun" && gameManager.isGameResetting)
        {
            Debug.Log("Skipping gun spawn because the game is being reset.");
            return false;
        }

        // Allow spawning of the item
        return true;
    }

    // Method to spawn the item at the appropriate location
    private void SpawnItem(Item item)
    {
        if (item.prefab != null)
        {
            Transform chosenSpawnPoint = spawnPoint; // Default to spawnPoint

            // Check the item's tag to determine the spawn point
            string itemTag = item.prefab.tag; // Capture the tag for debugging
            Debug.Log($"Checking tag for item: {item.itemName}, Tag: {itemTag}");

            if (itemTag == "Pistol")
            {
                chosenSpawnPoint = pistolSpawnPoint;
                Debug.Log($"Selected spawn point for {item.itemName}: {pistolSpawnPoint.position}");
            }
            else if (itemTag == "Holster")
            {
                chosenSpawnPoint = holsterSpawnPoint;
                Debug.Log($"Selected spawn point for {item.itemName}: {holsterSpawnPoint.position}");
            }
            else
            {
                Debug.Log($"Using default spawn point for {item.itemName}: {spawnPoint.position}");
            }

            // Check if the game is resetting and if the item should be spawned
            if (ShouldSpawnItem(item))
            {
                // Instantiate at the chosen spawn point
                GameObject spawnedItem = Instantiate(item.prefab, chosenSpawnPoint.position, Quaternion.identity);
                spawnedItem.name = item.itemName; // Set the name for clarity
                Debug.Log($"Spawned {item.itemName} at {chosenSpawnPoint.position}");
            }
        }
        else
        {
            Debug.LogWarning($"Item {item.itemName} has no prefab assigned!");
        }
    }
}
