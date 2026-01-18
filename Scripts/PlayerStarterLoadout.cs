using UnityEngine;

public class PlayerStarterLoadout : MonoBehaviour
{
    public Inventory inventory;
    public PlayerEquipment equipment;

    [Header("Start Items")]
    public ItemData startWeaponItem2;

    void Start()
    {
        GiveStartItems();
    }

    void GiveStartItems()
    {
        if (startWeaponItem2 == null)
            return;

        // 1. кладём в инвентарь
        inventory.AddItem(startWeaponItem2);

        // 2. кладём в слот ITEM2
        equipment.SetItemToSlot(EquipSlot.Item2, startWeaponItem2);
    }
}
