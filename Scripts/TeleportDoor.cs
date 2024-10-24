using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    public Transform teleportPositionShop;
    public GameObject Player;
    public PlayerController PlayerController;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.goToStore && Input.GetKeyDown("e")) // Ensure goToStore is true and e is pressed
        {
            teleportPositionShop = teleportPositionShop.GetComponent<Transform>();
            if (teleportPositionShop != null)  // Check that teleportPosition is assigned
            {
                Player.transform.position = teleportPositionShop.transform.position;
                //teleportPositionShop = null;
            }
        }
    }
}
/*
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
*/