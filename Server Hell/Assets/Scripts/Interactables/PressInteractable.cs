using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PressInteractable : MonoBehaviour, IInteractable
{
    public string interactionName;

    public event Action Interacted;

	public bool Interactable { get; set; } = true;

	public string InteractionName => interactionName;

    public void Interact(InputAction action)
    {
        if (!Interactable && Interacted != null)
            Interacted();
    }
}
