using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public GameObject myHands; // Reference to your hands (position where object goes)
    public bool canPickup = false; // Flag to check if you can pick up
    public GameObject heldItem; // Reference to the currently held item
    public bool hasItem = false; // Flag to check if you have an item in hand
    public PlayerController playerInteract;  // Reference to the PlayerInteract script
    public Vector3 originalScale;

    // This will hold the item data of the currently picked item
    public Item heldItemStats;

    void Update()
    {
        if (playerInteract.canPickup && playerInteract.currentInteractableObject != null && !hasItem)
        {
            GameObject ObjectToPickUp = playerInteract.currentInteractableObject;

            if (Input.GetKeyDown("e"))
            {
                PickUpItem(ObjectToPickUp);
            }
        }
    }

    private void PickUpItem(GameObject ObjectToPickUp)
    {
        // Get the ItemHolder component to access the item data
        ItemHolder itemHolder = ObjectToPickUp.GetComponent<ItemHolder>();

        if (itemHolder != null && itemHolder.item != null)
        {
            heldItemStats = itemHolder.item; // Assign the item data directly
            originalScale = ObjectToPickUp.transform.localScale;

            // Disable physics and collider on the object
            ObjectToPickUp.GetComponent<Rigidbody>().isKinematic = true;
            ObjectToPickUp.GetComponent<Collider>().enabled = false;

            heldItem = ObjectToPickUp; // Set the object as the currently held item

            // Parent the object to the player's hand
            heldItem.transform.SetParent(myHands.transform, false);

            // Reset the object's position and rotation
            PickedupRotation interactableObject = heldItem.GetComponent<PickedupRotation>();
            if (interactableObject != null)
            {
                heldItem.transform.localPosition = interactableObject.positionOffset;
                heldItem.transform.localRotation = Quaternion.Euler(interactableObject.rotationOffset);
            }
            else
            {
                heldItem.transform.localPosition = new Vector3(0, 0, 0.55f);
                heldItem.transform.localRotation = Quaternion.identity;  // Default rotation
            }

            // Restore the original scale
            heldItem.transform.localScale = originalScale;

            hasItem = true; // Mark that the player is holding an item
            Debug.Log($"Picked up: {heldItemStats.itemName}");
        }
        else
        {
            Debug.LogWarning("Picked up object does not have an ItemHolder component or item data!");
        }
    }
}

