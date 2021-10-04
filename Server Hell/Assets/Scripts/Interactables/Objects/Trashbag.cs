using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashbag : EquipInteractable
{
	public event Action<Trashbag> TrashThrownAway;

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
}
