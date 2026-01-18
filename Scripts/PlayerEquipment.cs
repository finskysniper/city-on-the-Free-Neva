using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [Header("Slots")]
    public ItemData item1;
    public ItemData item2;
    public ItemData armor;

    [Header("State")]
    public EquipSlot activeSlot = EquipSlot.Item1;
    public WeaponState weaponState = WeaponState.Unequipped;

    [Header("Refs")]
    public Animator animator;

    ItemData currentWeapon;

    void Update()
    {
        HandleSlotInput();
    }

    void HandleSlotInput()
    {
        if (weaponState != WeaponState.Equipped)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchSlot(EquipSlot.Item1);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchSlot(EquipSlot.Item2);
    }

    public void SwitchSlot(EquipSlot slot)
    {
        if (activeSlot == slot)
            return;

        activeSlot = slot;

        ItemData target = GetItemInSlot(slot);

        if (target == currentWeapon)
            return;

        StartUnequip(target);
    }

    ItemData GetItemInSlot(EquipSlot slot)
    {
        return slot switch
        {
            EquipSlot.Item1 => item1,
            EquipSlot.Item2 => item2,
            _ => null
        };
    }

    void StartUnequip(ItemData next)
    {
        weaponState = WeaponState.Unequipping;
        animator.Play("Weapon_Hide");

        Invoke(nameof(FinishUnequip), 0.4f);
        pendingWeapon = next;
    }

    ItemData pendingWeapon;

    void FinishUnequip()
    {
        currentWeapon = null;

        if (pendingWeapon != null)
            StartEquip(pendingWeapon);
        else
            weaponState = WeaponState.Unequipped;
    }

    void StartEquip(ItemData weapon)
    {
        currentWeapon = weapon;
        weaponState = WeaponState.Equipping;

        animator.Play("Weapon_Draw");

        Invoke(nameof(FinishEquip), 0.4f);
    }

    void FinishEquip()
    {
        weaponState = WeaponState.Equipped;
    }

    public void SetItemToSlot(EquipSlot slot, ItemData item)
    {
        switch (slot)
        {
            case EquipSlot.Item1:
                item1 = item;
                break;

            case EquipSlot.Item2:
                item2 = item;
                break;

            case EquipSlot.Armor:
                armor = item;
                break;
        }

        // если кладём в активный слот — сразу переключаемся
        if (slot == activeSlot)
        {
            SwitchSlot(slot);
        }
    }

}
