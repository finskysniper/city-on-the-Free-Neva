using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;

    [HideInInspector]
    public Vector2Int hex;

    void Start()
    {
        HexGrid grid = FindObjectOfType<HexGrid>();
        SnapToHex(grid);
    }

    void SnapToHex(HexGrid grid)
    {
        // 1. определяем хекс по текущей позиции
        hex = grid.WorldToHex(transform.position);

        // 2. получаем центр этого хекса
        Vector3 center = grid.HexToWorld(hex.x, hex.y);

        // 3. сохраняем высоту объекта
        center.y = transform.position.y;

        // 4. СТАВИМ ОБЪЕКТ В ЦЕНТР
        transform.position = center;
    }
}
