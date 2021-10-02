using UnityEngine.InputSystem;

public interface IInteractable
{
    string InteractionName { get; }

    void Interact(InputAction action);
}
