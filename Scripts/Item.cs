using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public GameObject prefab; // Prefab of the item
    public int sellprice;
    public float price;

    // Override Equals and GetHashCode for proper comparison
    public override bool Equals(object obj)
    {
        if (obj is Item other)
        {
            return itemName == other.itemName; // Compare item names
        }
        return false;
    }

    public override int GetHashCode()
    {
        return itemName.GetHashCode();
    }
}
