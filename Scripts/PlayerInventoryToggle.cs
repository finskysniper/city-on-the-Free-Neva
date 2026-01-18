using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventoryToggle : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject inventoryPanel;

    [Header("Input")]
    [SerializeField] InputActionReference cancelAction;

    bool isOpen;

    void Awake()
    {
        inventoryPanel.SetActive(false);
        ApplyState(false);
    }

    void OnEnable()
    {
        cancelAction.action.performed += OnCancel;
        cancelAction.action.Enable();
    }

    void OnDisable()
    {
        cancelAction.action.performed -= OnCancel;
        cancelAction.action.Disable();
    }

    void OnCancel(InputAction.CallbackContext ctx)
    {
        Toggle();
    }

    void Toggle()
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
