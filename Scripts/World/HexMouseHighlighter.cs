using UnityEngine;
using UnityEngine.InputSystem;

public class HexMouseHighlighter : MonoBehaviour
{
    public HexGrid grid;
    public LayerMask groundMask;

    HexTile currentTile;
    Vector2Int lastHex = new Vector2Int(999, 999);

    void Update()
    {
        // если не режим ходьбы — вообще ничего не делаем
        if (!CursorModeController.Instance.IsMovementMode())
            return;

        if (Mouse.current == null)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
        {
            Vector2Int hex = grid.WorldToHex(hit.point);

            if (hex != lastHex)
            {
                Clear();
                HighlightHex(hex);
                lastHex = hex;
            }
        }
        else
        {
            Clear();
            lastHex = new Vector2Int(999, 999);
        }
    }

    void HighlightHex(Vector2Int coords)
    {
        if (!grid.HasCell(coords.x, coords.y))
            return;

        foreach (Transform child in grid.transform)
        {
            HexTile tile = child.GetComponent<HexTile>();
            if (tile != null && tile.q == coords.x && tile.r == coords.y)
            {
                tile.Highlight(true);
                currentTile = tile;
                return;
            }
        }
    }

    public void Clear()
    {
        if (currentTile != null)
        {
            currentTile.Highlight(false);
            currentTile = null;
        }

        lastHex = new Vector2Int(999, 999);
    }
}
