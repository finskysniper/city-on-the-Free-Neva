using UnityEngine;

public class TestInteract : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Chest opened");
    }
}
