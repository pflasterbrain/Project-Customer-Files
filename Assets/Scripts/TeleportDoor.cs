using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    public Transform otherDoor; // The other door to teleport to
    public GameObject player; // Reference to the player GameObject
    public Vector3 teleportOffset = new Vector3(1f, 0f, 0f); // Offset position relative to the door

    // Update is called once per frame
    void Update()
    {
        // Check if the player is pressing 'E' and the player is near the door
        if (Input.GetKeyDown(KeyCode.E) && IsPlayerNear())
        {
            TeleportPlayer();
        }
    }

    private bool IsPlayerNear()
    {
        // Simple distance check to see if the player is close enough to interact with the door
        return Vector3.Distance(player.transform.position, transform.position) < 3f; // Adjust the distance as needed
    }

    private void TeleportPlayer()
    {
        if (otherDoor != null)
        {
            // Calculate the new position based on the other door's position and the specified offset
            Vector3 newPosition = otherDoor.position + teleportOffset;
            player.transform.position = newPosition;
        }
    }
}