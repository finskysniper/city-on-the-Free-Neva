using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Limits")]
    public float maxWeight = 30f;

    public List<ItemData> items = new();
    public float CurrentWeight { get; private set; }

    public bool CanAdd(ItemData item)
    {
        return CurrentWeight + item.weight <= maxWeight;
    }

    public bool AddItem(ItemData item)
    {
        if (!CanAdd(item))
            return false;

        items.Add(item);
        CurrentWeight += item.weight;
        return true;
    }

    public void RemoveItem(ItemData item)
    {
        if (items.Remove(item))
            CurrentWeight -= item.weight;
    }
}
