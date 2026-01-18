using UnityEngine;
using UnityEngine.InputSystem;

public class FreeCameraController : MonoBehaviour
{
    [Header("Start Setup")]
    public Transform player;
    public float startDistance = 12f;

    [Header("Movement")]
    public float moveSpeed = 12f;

    [Header("Bounds")]
    public Vector2 minBounds = new Vector2(-9999, -9999);
    public Vector2 maxBounds = new Vector2(9999, 9999);

    [Header("Edge Scroll Percentages")]
    [Range(0f, 0.5f)] public float leftZone = 0.05f;
    [Range(0f, 0.5f)] public float rightZone = 0.05f;
    [Range(0f, 0.5f)] public float topZone = 0.05f;
    [Range(0f, 0.5f)] public float bottomZone = 0.15f; // ↓ шире, и это правильно

    void Start()
    {
        // фиксированный тактический угол
        transform.rotation = Quaternion.Euler(30f, -30f, 0f);

        if (player != null)
        {
            Vector3 dir = transform.forward;
            transform.position = player.position - dir * startDistance;
        }
    }

    void LateUpdate()
    {
        if (Mouse.current == null || Keyboard.current == null)
            return;

        Vector3 move = Vector3.zero;

        // ===== НАПРАВЛЕНИЯ ПО ЭКРАНУ =====
        Camera cam = Camera.main;

        Vector3 screenForward = Vector3.ProjectOnPlane(cam.transform.up, Vector3.up).normalized;
        Vector3 screenRight = Vector3.ProjectOnPlane(cam.transform.right, Vector3.up).normalized;

        // ===== НАПРАВЛЕНИЯ ПО МИРУ (ДЛЯ WASD) =====
        Vector3 worldForward = transform.forward;
        worldForward.y = 0;
        worldForward.Normalize();

        Vector3 worldRight = transform.right;
        worldRight.y = 0;
        worldRight.Normalize();

        // ===== ⌨️ WASD / СТРЕЛКИ (ПО МИРУ) =====
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            move += worldForward;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            move -= worldForward;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            move += worldRight;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            move -= worldRight;

        // ===== 🖱 EDGE SCROLL (ПО ЭКРАНУ, ПО ПРОЦЕНТАМ) =====
        Vector2 mouse = Mouse.current.position.ReadValue();

        float leftZone = Screen.width * 0.05f;
        float rightZone = Screen.width * 0.95f;
        float topZone = Screen.height * 0.95f;
        float bottomZone = Screen.height * 0.15f; // ↓ ВНИЗ ШИРЕ — И ЭТО ПРАВИЛЬНО

        if (mouse.x <= leftZone)
            move -= screenRight;
        else if (mouse.x >= rightZone)
            move += screenRight;

        if (mouse.y <= bottomZone)
            move -= screenForward;     // ↓ НИЗ ЭКРАНА
        else if (mouse.y >= topZone)
            move += screenForward;     // ↑ ВЕРХ ЭКРАНА

        // ===== ПРИМЕНЕНИЕ ДВИЖЕНИЯ =====
        if (move.sqrMagnitude > 0.0001f)
        {
            Vector3 newPos = transform.position +
                             move.normalized * moveSpeed * Time.deltaTime;

            newPos.x = Mathf.Clamp(newPos.x, minBounds.x, maxBounds.x);
            newPos.z = Mathf.Clamp(newPos.z, minBounds.y, maxBounds.y);

            transform.position = newPos;
        }
    }
}
