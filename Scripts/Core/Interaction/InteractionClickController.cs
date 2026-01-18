using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InteractionClickController : MonoBehaviour
{
    public LayerMask interactableMask;

    void Update()
    {
        if (Mouse.current == null)
            return;

        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        // ❗ если клик по UI — выходим (HUD на будущее)
        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = Camera.main.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, interactableMask))
        {
            IInteractable interactable =
                hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
