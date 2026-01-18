using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    public int radius = 5;
    public float hexSize = 1f;

    [Header("Visuals")]
    public GameObject hexVisualPrefab;

    // axial coords → cell
    public Dictionary<Vector2Int, HexCell> cells =
        new Dictionary<Vector2Int, HexCell>();

    // 6 направлений для axial (flat-top)
    public static readonly Vector2Int[] directions =
    {
        new Vector2Int(+1,  0),
        new Vector2Int(+1, -1),
        new Vector2Int( 0, -1),
        new Vector2Int(-1,  0),
        new Vector2Int(-1, +1),
        new Vector2Int( 0, +1),
    };

    void Start()
    {
        GenerateGrid();
    }

    // =========================
    // GENERATION
    // =========================

    void GenerateGrid()
    {
        cells.Clear();

        // очистка старых тайлов
        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);

        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);

            for (int r = r1; r <= r2; r++)
            {
                HexCell cell = new HexCell(q, r);
                cells.Add(new Vector2Int(q, r), cell);

                if (hexVisualPrefab != null)
                {
                    Vector3 pos = HexToWorld(q, r);
                    GameObject go = Instantiate(
                        hexVisualPrefab,
                        pos,
                        Quaternion.identity,
                        transform
                    );

                    HexTile tile = go.GetComponent<HexTile>();
                    if (tile != null)
                        tile.Init(q, r);
                }
            }
        }

        Debug.Log($"HexGrid generated: {cells.Count} cells");
    }

    // =========================
    // COORDINATES (FLAT-TOP)
    // =========================

    // axial → world
    public Vector3 HexToWorld(int q, int r)
    {
        float x = hexSize * 1.5f * q;
        float z = hexSize * Mathf.Sqrt(3f) * (r + q * 0.5f);
        return new Vector3(x, 0f, z);
    }

    // world → axial
    public Vector2Int WorldToHex(Vector3 position)
    {
        float q = (2f / 3f * position.x) / hexSize;
        float r = (-1f / 3f * position.x + Mathf.Sqrt(3f) / 3f * position.z) / hexSize;
        return CubeRound(q, r);
    }

    Vector2Int CubeRound(float q, float r)
    {
        float x = q;
        float z = r;
        float y = -x - z;

        int rx = Mathf.RoundToInt(x);
        int ry = Mathf.RoundToInt(y);
        int rz = Mathf.RoundToInt(z);

        float xDiff = Mathf.Abs(rx - x);
        float yDiff = Mathf.Abs(ry - y);
        float zDiff = Mathf.Abs(rz - z);

        if (xDiff > yDiff && xDiff > zDiff)
            rx = -ry - rz;
        else if (yDiff > zDiff)
            ry = -rx - rz;
        else
            rz = -rx - ry;

        return new Vector2Int(rx, rz);
    }

    // =========================
    // QUERIES
    // =========================

    public bool HasCell(int q, int r)
    {
        return cells.ContainsKey(new Vector2Int(q, r));
    }

    public HexCell GetCell(int q, int r)
    {
        cells.TryGetValue(new Vector2Int(q, r), out HexCell cell);
        return cell;
    }

    // =========================
    // PATHFINDING (BFS)
    // =========================

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        if (start == goal)
            return new List<Vector2Int>();

        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        frontier.Enqueue(start);

        Dictionary<Vector2Int, Vector2Int> cameFrom =
            new Dictionary<Vector2Int, Vector2Int>();

        cameFrom[start] = start;

        while (frontier.Count > 0)
        {
            Vector2Int current = frontier.Dequeue();

            if (current == goal)
                break;

            foreach (var dir in directions)
            {
                Vector2Int next = current + dir;

                if (!HasCell(next.x, next.y))
                    continue;

                HexCell cell = GetCell(next.x, next.y);
                if (cell == null || !cell.walkable)
                    continue;

                if (cameFrom.ContainsKey(next))
                    continue;

                frontier.Enqueue(next);
                cameFrom[next] = current;
            }
        }

        if (!cameFrom.ContainsKey(goal))
            return null;

        // восстановление пути
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int step = goal;

        while (step != start)
        {
            path.Add(step);
            step = cameFrom[step];
        }

        path.Reverse();
        return path;
    }
}

// =========================
// DATA
// =========================
