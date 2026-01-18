using UnityEngine;
using UnityEngine.InputSystem;

public class CursorModeController : MonoBehaviour
{
    public static CursorModeController Instance;

    public CursorMode currentMode = CursorMode.Movement;

    [Header("Controllers")]
    public HexMouseHighlighter hexHighlighter;
    public HexClickController hexClickController;
    public InteractionClickController interactionClickController;

    [Header("Edge Scroll Percentages")]
    [Range(0f, 0.5f)] public float leftZone = 0.05f;
    [Range(0f, 0.5f)] public float rightZone = 0.05f;
    [Range(0f, 0.5f)] public float topZone = 0.05f;
    [Range(0f, 0.5f)] public float bottomZone = 0.15f; // ↓ шире, и это правильно

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SetMode(CursorMode.Movement);
    }

    void Update()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            ToggleMode();
        }
    }

    void ToggleMode()
    {
        if (currentMode == CursorMode.Movement)
            SetMode(CursorMode.Interaction);
        else
            SetMode(CursorMode.Movement);
    }

    void SetMode(CursorMode mode)
    {
        currentMode = mode;

        bool movement = mode == CursorMode.Movement;

        // ❗ ВАЖНО: НИКАКОГО CursorLockMode.Locked
        Cursor.visible = !movement;
        Cursor.lockState = CursorLockMode.None;

        // режим ходьбы
        if (hexHighlighter != null)
        {
            if (!movement)
                hexHighlighter.Clear(); // 🔥 ГАСИМ ПОДСВЕТКУ

            hexHighlighter.enabled = movement;
        }
        if (hexClickController != null)
            hexClickController.enabled = movement;

        // режим взаимодействия
        if (interactionClickController != null)
            interactionClickController.enabled = !movement;

        Debug.Log($"Cursor mode: {currentMode}");
    }

    public bool IsMovementMode()
    {
        return currentMode == CursorMode.Movement;
    }
}
