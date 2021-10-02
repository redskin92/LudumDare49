using UnityEngine;
using UnityEngine.InputSystem;

public class EquipInteractable : MonoBehaviour, IInteractable
{
	public string interactionName = "Equip";

	public string InteractionName => interactionName;

	private Rigidbody rbody;


	public void Awake()
	{
		rbody = GetComponent<Rigidbody>(); 
	}

	public void DropItem()
	{
		rbody.isKinematic = false;
	}

	public void Interact(InputAction action)
	{
		var player = GameObject.FindGameObjectWithTag("Player");
		var inventory = player.GetComponent<PlayerInventory>();

		if(inventory.EquipItem(this))
		{
			rbody.isKinematic = true;
		}
	}

	public virtual void ItemUse(InputAction action)
	{
		Debug.Log("USING ITEM");
	}
}
