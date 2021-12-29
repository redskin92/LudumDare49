using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Trashbag : EquipInteractable
{
	public event Action<Trashbag> TrashThrownAway;
	public event Action TrashPickedUp;
	public event Action TrashReleased;

	bool processed = false;

	private void OnTriggerEnter(Collider other)
	{
		if (processed) return;

		if (other.gameObject.name == "TrashChute")
		{
			FireThrownAway();
		}
	}

	private void Update()
	{
		if (processed) return;	

		if(this.transform.position.y < -20f)
		{
			FireThrownAway();
		}
	}

	private void FireThrownAway()
	{
		if (TrashThrownAway != null)
			TrashThrownAway(this);

		processed = true;
	}


	public override void OnInteractSuccess()
	{
		base.OnInteractSuccess();

		if (TrashPickedUp != null)
			TrashPickedUp();
	}

	public override void DropItem(Vector3 force)
	{
		base.DropItem(force);

		if (TrashReleased != null)
			TrashReleased();
	}
}
