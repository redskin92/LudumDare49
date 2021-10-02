using UnityEngine;

public class EquipInteractable : MonoBehaviour, IInteractable
{
	public string interactionName = "Equip";

	public string InteractionName => interactionName;

	public void Interact()
	{
		Debug.Log("Interaction detected!  Hey there :)");
	}
}
