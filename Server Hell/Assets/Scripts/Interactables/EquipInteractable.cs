using UnityEngine;
using UnityEngine.InputSystem;

public class EquipInteractable : MonoBehaviour, IInteractable
{
	public string interactionName = "Equip";

	public string InteractionName => interactionName;

	public void Interact(InputAction action)
	{
		Debug.Log("Interaction detected!  Hey there :)");
	}
}
