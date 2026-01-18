using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HexTile : MonoBehaviour
{
    public int q;
    public int r;

    [Header("Outline")]
    public GameObject outlinePrefab;

    GameObject outlineInstance;

    public void Init(int q, int r)
    {
        this.q = q;
        this.r = r;
    }

    void Awake()
    {
        // основной меш тайла можно скрыть
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null)
            mr.enabled = false;
    }

    public void Highlight(bool state)
    {
        if (state)
        {
            if (outlineInstance == null)
            {
                outlineInstance = Instantiate(
                    outlinePrefab,
                    transform.position + Vector3.up * 0.05f,
                    transform.rotation,
                    transform
                );
            }

            outlineInstance.SetActive(true);
        }
        else
        {
            if (outlineInstance != null)
                outlineInstance.SetActive(false);
        }
    }
}
