using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TeleportDoorShop : MonoBehaviour
{
    public Transform teleportPositionHouse; // Where to teleport the player when using doors
    public GameObject Player; // Reference to the player
    public PlayerController PlayerController; // Reference to PlayerController
    public VideoPlayer VideoPlayerRobbed; // Video for the robbed scenario
    public VideoPlayer VideoPlayerArrested; // Video for the arrested scenario
    public float stopAfterSeconds = 12f; // Time to stop the video after it starts
    public Pickup playerPickup; // Reference to the player's pickup
    public Transform gameManager;
    public PlayerInventory playerInventory;
    public GameManager GameManager;

    void Update()
    {
        bool hasHolster = playerInventory.HasItem("Holster");
        Debug.Log(hasHolster);
        // Check for the interaction to teleport when "E" is pressed
        if (Input.GetKeyDown("e"))
        {
            if (PlayerController.goToHouse)
            {
                HandleVideoPlayback(); // Handle video playback if at the door
            }
            else if (PlayerController.goToStore) // Assuming this is for the shop door
            {
                TeleportPlayer(teleportPositionHouse); // Teleport the player to the house
            }
        }

        // Check if the back button is pressed and reload the scene
        /*if (Input.GetKeyDown(KeyCode.Escape)) // Assuming Escape is the back button
        {
            ReloadScene();
        }*/
    }

    private void HandleVideoPlayback()
    {
        // Use the HasItem method to check for the holster
        bool hasHolster = playerInventory.HasItem("Holster");

        // Debugging output
        Debug.Log("Current Inventory Items:");
        foreach (var item in playerInventory.ownedItems)
        {
            Debug.Log($"Item: {item.itemName}"); // Print each item's name
        }

        Debug.Log($"Has Holster: {hasHolster}");

        if (playerPickup.heldItem == null)
        {
            VideoPlayerRobbed.Play();
            StartCoroutine(StopVideoAfterDelay(VideoPlayerRobbed));
        }
        else if (playerPickup.hasItem && playerPickup.heldItem != null)
        {
            // Check if the held item is tagged as "Pistol"
            if (!playerPickup.heldItem.CompareTag("Pistol"))
            {
                VideoPlayerRobbed.Play();
                StartCoroutine(StopVideoAfterDelay(VideoPlayerRobbed));
            }
            else if (playerPickup.heldItem.CompareTag("Pistol") && !hasHolster)
            {
                VideoPlayerArrested.Play();
                StartCoroutine(StopVideoAfterDelay(VideoPlayerArrested));
            }
        }

        if (!playerPickup.heldItem.CompareTag("Pistol") && hasHolster)
        {
            TeleportPlayer(teleportPositionHouse);
        }
    }



    private IEnumerator StopVideoAfterDelay(VideoPlayer videoPlayer)
    {
        // Wait for the specified number of seconds
        yield return new WaitForSeconds(stopAfterSeconds);
        Debug.Log("Stopped video");
        // Stop the video
        videoPlayer.Stop();
        videoPlayer.enabled = false;

        // Reload the scene after the video finishes
        ReloadScene();
    }

    private void TeleportPlayer(Transform destination)
    {
        if (destination != null)
        {
            Player.transform.position = destination.position; // Teleport the player
            Debug.Log("Player teleported to: " + destination.position);
        }
        else
        {
            Debug.LogWarning("Teleport position is not assigned!");
        }
    }

    private void ReloadScene()
    {
        // Optionally, reset the GameManager directly if needed, instead of destroying it
        GameManager.Instance.FullGameReset(); // Reset values before loading the new scene

        // Reload the current scene
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    private bool HasHolster()
    {
        foreach (var item in playerInventory.ownedItems)
        {
            if (item.itemName == "Holster")
            {
                return true; // Found the holster
            }
        }
        return false; // Holster not found
    }
}
