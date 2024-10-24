using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PutDown : MonoBehaviour
{
    public PlayerController playerInteract;
    public Pickup pickupScript;  // Reference to the Pickup script to access the held item

    void Update()
    {
        // Check if the player can put down an item and is holding something
        if (playerInteract.canPutdown && pickupScript.hasItem && playerInteract.currentPutdownObject != null)
        {
            if (Input.GetKeyDown("e"))
            {
                PutDownItem();
            }
        }
    }

    private void PutDownItem()
    {
        // Reference to the item currently being held
        GameObject heldItem = pickupScript.heldItem;

        if (heldItem != null)
        {
            // Re-enable physics and collider on the object
            heldItem.GetComponent<Rigidbody>().isKinematic = false;
            heldItem.GetComponent<Collider>().enabled = true;

            // Parent the object to the "PutDown" object
            heldItem.transform.SetParent(playerInteract.currentPutdownObject.transform, true);

            // Get the PutDownPosition component from the "PutDown" object (if it exists)
            PutdownPosition putDownPosition = playerInteract.currentPutdownObject.GetComponent<PutdownPosition>();

            // Apply the position and rotation offsets
            if (putDownPosition != null)
            {
                heldItem.transform.localPosition = putDownPosition.positionOffset;
                heldItem.transform.localRotation = Quaternion.Euler(putDownPosition.rotationOffset);
            }
            else
            {
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;
            }

            // Optionally restore the original scale
            heldItem.transform.localScale = pickupScript.originalScale;

            // Clear the reference to the held item
            pickupScript.hasItem = false;
            pickupScript.heldItem = null;

            Debug.Log("Item put down on: " + playerInteract.currentPutdownObject.name);
        }
    }
}