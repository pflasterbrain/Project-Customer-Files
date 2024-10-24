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
    // Add other properties as needed
}