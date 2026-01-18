using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryPanel;

    bool isOpen;

    void Start()
    {
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Toggle();
        }
    }

    void Toggle()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        // курсор
        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;

        // пауза (по желанию)
        Time.timeScale = isOpen ? 0f : 1f;
    }
}
