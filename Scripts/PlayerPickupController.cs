using UnityEngine;

public class PlayerPickupController : MonoBehaviour
{
    public Inventory inventory;
    public Animator animator;
    public HexPlayerMovement movement;
    public HexGrid grid;

    ItemPickup target;
    bool isPickingUp;

    public void StartPickup(ItemPickup item)
    {
        if (isPickingUp)
            return;

        target = item;

        // идём В ХЕКС предмета
        movement.MoveToHex(item.hex.x, item.hex.y);
    }

    void Update()
    {
        if (target == null || isPickingUp)
            return;

        // проверяем: мы В ТОЙ ЖЕ КЛЕТКЕ?
        Vector2Int playerHex = grid.WorldToHex(transform.position);

        if (playerHex == target.hex)
        {
            BeginPickup();
        }
    }

    void BeginPickup()
    {
        isPickingUp = true;

        animator.Play("Pickup");

        Invoke(nameof(FinishPickup), 0.8f);
    }

    void FinishPickup()
    {
        if (target == null)
            return;

        if (inventory.AddItem(target.itemData))
        {
            Destroy(target.gameObject);
        }

        target = null;
        isPickingUp = false;
    }
}
