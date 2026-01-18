using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [Header("Base Info")]
    public string itemName;

    [TextArea]
    public string description;

    public Sprite icon;

    [Header("Inventory")]
    public float weight;
}
