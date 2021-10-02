using UnityEngine.InputSystem;

public interface IInteractable
{
    string InteractionName { get; }

	bool Interactable { get; set; }

    void Interact(InputAction action);
}
