using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItemView : MonoBehaviour
{
    public TMP_Text nameText;
    public Image icon;

    ItemData data;
    InventoryUI ui;

    public void Init(ItemData item, InventoryUI inventoryUI)
    {
        data = item;
        ui = inventoryUI;

        nameText.text = item.itemName;
        icon.sprite = item.icon;
    }

    public void OnClick()
    {
        ui.ShowItem(data);
    }
}
