using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolsterInteract : MonoBehaviour
{
    public Pickup pickupScript;
    public PlayerController playerInteract;  // Reference to PlayerController for holster and hip transforms
    public GameObject playerBody;
    void Update()
    {
        // Ensure the player can holster and is holding an item
        if (playerInteract.canHolster && pickupScript.hasItem)
        {
            if (Input.GetKeyDown("e"))
            {
                HolsterItem();  // Step 1: Holster the item
                
            }
        }

        // Additional input to move the holster with gun to the hip
        if (playerInteract.canMoveHolsterToHip && playerInteract.currentHolster != null)
        {
            if (Input.GetKeyDown("r"))  // Press 'r' to move holster with gun to hip
            {
                MoveHolsterToHip();  // Step 2: Move the holster to the player's hip
            }
        }
    }

    private void HolsterItem()
    {
        GameObject heldItem = pickupScript.heldItem;

        if (heldItem != null)
        {
            // Re-enable physics and collider on the item
            heldItem.GetComponent<Rigidbody>().isKinematic = false;
            heldItem.GetComponent<Collider>().enabled = true;

            // Set the item's parent to the holster (which is the current holster object in playerInteract)
            Transform holsterTransform = playerInteract.currentHolster.transform;

            // Attach the gun to the holster
            heldItem.transform.SetParent(holsterTransform, true);

            // Apply position and rotation offsets based on the holster's settings
            HolsterPosition holsterPosition = playerInteract.currentHolster.GetComponent<HolsterPosition>();
            if (holsterPosition != null)
            {
                heldItem.transform.localPosition = holsterPosition.positionOffset;
                heldItem.transform.localRotation = Quaternion.Euler(holsterPosition.rotationOffset);
            }
            else
            {
                // Default positioning if no holster position is specified
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;
            }

            // Restore the original scale of the gun
            heldItem.transform.localScale = pickupScript.originalScale;

            // Clear the held item state after holstering
            pickupScript.hasItem = false;
            pickupScript.heldItem = null;
            playerInteract.canMoveHolsterToHip = true;
            Debug.Log("Gun placed in holster: " + playerInteract.currentHolster.name);
        }
    }

    private void MoveHolsterToHip()
    {
        GameObject holster = playerInteract.currentHolster;

        if (holster != null)
        {
            holster.GetComponent<Rigidbody>().isKinematic = false;
            holster.GetComponent<Collider>().enabled = false;
            Debug.Log("Before setting heldItem: " + pickupScript.heldItem);
            pickupScript.heldItem = holster;
            pickupScript.hasItem = true;
            Debug.Log("After setting heldItem: " + pickupScript.heldItem);

            // Attach the holster (with the gun inside) to the player's hip
            //Transform hipTransform = playerInteract.hipTransform;  // Reference to the player's hip transform
            Transform bodyTransform = playerInteract.bodyTransform;
            holster.transform.SetParent(bodyTransform, false);
            Debug.Log("Body/Hip Transform: " + bodyTransform.name);
            // Apply position and rotation offsets to attach the holster to the hip
            PickedupRotation hipPosition = bodyTransform.GetComponent<PickedupRotation>();
            if (hipPosition != null)
            {
                holster.transform.localPosition = hipPosition.positionOffset;
                holster.transform.localRotation = Quaternion.Euler(hipPosition.rotationOffset);
            }
            else
            {
                // Default positioning if no hip position is provided
                holster.transform.localPosition = Vector3.zero;
                holster.transform.localRotation = Quaternion.identity;
            }
            

            Debug.Log("Holster (with gun) moved to hip.");
        }
    }
}