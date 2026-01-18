using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public PlayerEquipment playerEquipment;

    ItemData selectedItem;

    [Header("List")]
    public Transform listRoot;
    public InventoryItemView itemPrefab;

    [Header("Preview")]
    public Image previewIcon;
    public TMP_Text previewName;
    public TMP_Text descriptionText;
    public TMP_Text weightText;

    void OnEnable()
    {
        Rebuild();
    }

    public void Rebuild()
    {
        foreach (Transform child in listRoot)
            Destroy(child.gameObject);

        foreach (var item in inventory.items)
        {
            var view = Instantiate(itemPrefab, listRoot);
            view.Init(item, this);
        }

        ClearPreview();
    }

    public void ShowItem(ItemData item)
    {
        selectedItem = item;

        previewIcon.sprite = item.icon;
        previewName.text = item.itemName;

        if (item is WeaponData weapon)
        {
            descriptionText.text =
                $"{weapon.description}\n\n" +
                $"Damage: {weapon.baseDamage}\n" +
                $"Weight: {weapon.weight}";
        }
        else
        {
            descriptionText.text =
                $"{item.description}\n\n" +
                $"Weight: {item.weight}";
        }
    }

    public void EquipToItem1()
    {
        if (selectedItem == null)
            return;

        playerEquipment.SetItemToSlot(EquipSlot.Item1, selectedItem);
    }

    public void EquipToItem2()
    {
        if (selectedItem == null)
            return;

        playerEquipment.SetItemToSlot(EquipSlot.Item2, selectedItem);
    }

    public void EquipArmor()
    {
        if (selectedItem == null)
            return;

        playerEquipment.SetItemToSlot(EquipSlot.Armor, selectedItem);
    }

    void ClearPreview()
    {
        previewIcon.sprite = null;
        previewName.text = "";
        descriptionText.text = "";
    }
}
