using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryToggleInput : MonoBehaviour
{
    [Header("UI")]
    public GameObject inventoryPanel;

    [Header("Input")]
    public InputActionReference toggleInventoryAction;

    bool isOpen;

    void OnEnable()
    {
        toggleInventoryAction.action.performed += OnToggle;
        toggleInventoryAction.action.Enable();
    }

    void OnDisable()
    {
        toggleInventoryAction.action.performed -= OnToggle;
        toggleInventoryAction.action.Disable();
    }

    void Start()
    {
        inventoryPanel.SetActive(false);
        ApplyState(false);
    }

    void OnToggle(InputAction.CallbackContext ctx)
    {
        isOpen = !isOpen;
        ApplyState(isOpen);
    }

    void ApplyState(bool open)
    {
        inventoryPanel.SetActive(open);

        Cursor.visible = open;
        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;

        Time.timeScale = open ? 0f : 1f;
    }
}
