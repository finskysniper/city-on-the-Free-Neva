using UnityEngine;
using UnityEngine.InputSystem;

public class HexClickController : MonoBehaviour
{
    public HexGrid grid;
    public HexPlayerMovement player;
    public LayerMask groundMask;

    void Update()
    {
        if (Mouse.current == null) return;

        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        Ray ray = Camera.main.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
        {
            Vector2Int hex = grid.WorldToHex(hit.point);

            if (!grid.HasCell(hex.x, hex.y))
                return;

            player.MoveToHex(hex.x, hex.y);
        }
    }
}
