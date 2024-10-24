using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Safe : MonoBehaviour
{
    public PlayerController playerInteract;
    public Pickup pickupScript;  // Reference to the Pickup script to access the held item

    void Update()
    {
        
        // Check if the player can put down an item and is holding something
        if (playerInteract.canSafe)
        {
            Debug.Log("Can Store");
            if (Input.GetKeyDown("e"))
            {
                Debug.Log("Storing");
                storeItem();
            }
        }
    }

    private void storeItem()
    {
        // Reference to the item currently being held
        GameObject heldItem = pickupScript.heldItem;
        
        if (heldItem != null)
        {
            // Re-enable physics and collider on the object
            heldItem.GetComponent<Rigidbody>().isKinematic = false;
            heldItem.GetComponent<Collider>().enabled = true;

            // Parent the object to the "PutDown" object
            heldItem.transform.SetParent(playerInteract.currentSafeObject.transform, true);

            // Get the PutDownPosition component from the "PutDown" object (if it exists)
            SafePosition safePosition = playerInteract.currentSafeObject.GetComponent<SafePosition>();

            // Apply the position and rotation offsets
            if (safePosition != null)
            {
                heldItem.transform.localPosition = safePosition.positionOffset;
                heldItem.transform.localRotation = Quaternion.Euler(safePosition.rotationOffset);
            }
            else
            {
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;
            }
            heldItem.transform.localScale = new Vector3(
    heldItem.transform.localScale.x * 0.3f,
    heldItem.transform.localScale.y * 0.3f,
   heldItem.transform.localScale.z * 0.3f
);
            playerInteract.gunSafeText.enabled = false;
            // Optionally restore the original scale
            
            
            // Clear the reference to the held item
            pickupScript.hasItem = false;
            pickupScript.heldItem = null;

            Debug.Log("Item put down on: " + playerInteract.currentPutdownObject.name);
        }
    }
}