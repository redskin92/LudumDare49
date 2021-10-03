using UnityEngine;
using UnityEngine.InputSystem;

public class EquipInteractable : MonoBehaviour, IInteractable
{
	public string interactionName = "Press 'E' to Equip";
	public virtual string useActionName => string.Empty;

	public string InteractionName => interactionName;

	private Rigidbody rbody;
	private Collider objectCollider;

	public bool Interactable { get; set; } = true;


	protected virtual void Awake()
	{
		rbody = GetComponent<Rigidbody>();
		objectCollider = GetComponent<Collider>();
	}

	public void DropItem(Vector3 force)
	{
		objectCollider.enabled = true;
		rbody.isKinematic = false;

		if(force != Vector3.zero)
			rbody.AddForce(force, ForceMode.Impulse);
	}

	public void Interact(InputAction action)
	{
		var player = GameObject.FindGameObjectWithTag("Player");
		var inventory = player.GetComponent<PlayerInventory>();

		if(inventory.EquipItem(this))
		{
			objectCollider.enabled = false;
			rbody.isKinematic = true;
		}
	}


	public virtual void ItemUsePressed(InputAction action) {	}

	public virtual void ItemUseReleased(InputAction action)	{	}
}
