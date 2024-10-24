using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class childSafe : MonoBehaviour
{
    private Transform objectTransform;
    private bool hasChild = false;

    void Start()
    {
        objectTransform = this.transform;

        // Ensure the object starts with the "Holster" tag
        if (this.tag != "Safe")
        {
            this.tag = "Safe";
        }

    }

    void Update()
    {
        // Check if the object has any children
        if (objectTransform.childCount > 1 && !hasChild)
        {
            // If a child is added, change the tag to "Pickup"
            //this.tag = "Pickup";
            hasChild = true;
            ToggleChildColliders(false);
            Debug.Log("Tag changed to Pickup");
        }
        else if (objectTransform.childCount == 1 && hasChild)
        {
            // If all children are removed, change the tag back to "Holster"
            //this.tag = "Holster";
            hasChild = false;
            ToggleChildColliders(true);
            Debug.Log("Tag changed to Holster");
        }
    }
    private void ToggleChildColliders(bool state)
    {
        foreach (Transform child in objectTransform)
        {
            Collider childCollider = child.GetComponent<Collider>();
            if (childCollider != null)
            {
                childCollider.enabled = state;
            }
        }
    }
}