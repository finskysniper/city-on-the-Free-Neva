using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotsUI : MonoBehaviour
{
    [Header("Refs")]
    public PlayerEquipment equipment;

    [Header("Item 1")]
    public Image item1Icon;
    public GameObject item1ActiveFrame;

    [Header("Item 2")]
    public Image item2Icon;
    public GameObject item2ActiveFrame;

    void OnEnable()
    {
        Refresh();
    }

    void Update()
    {
        // дешево, но надежно
        Refresh();
    }

    void Refresh()
    {
        UpdateSlot(
            equipment.item1,
            item1Icon,
            item1ActiveFrame,
            equipment.activeSlot == EquipSlot.Item1
        );

        UpdateSlot(
            equipment.item2,
            item2Icon,
            item2ActiveFrame,
            equipment.activeSlot == EquipSlot.Item2
        );
    }

    void UpdateSlot(ItemData item, Image icon, GameObject frame, bool isActive)
    {
        if (item != null)
        {
            icon.enabled = true;
            icon.sprite = item.icon;
        }
        else
        {
            icon.enabled = false;
            icon.sprite = null;
        }

        frame.SetActive(isActive);
    }
}
