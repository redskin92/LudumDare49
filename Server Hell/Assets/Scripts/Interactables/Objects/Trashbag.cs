using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashbag : EquipInteractable
{
	public event Action<Trashbag> TrashThrownAway;
	public AudioSource completedSound;

	bool processed = false;

	private void OnTriggerEnter(Collider other)
	{
		if (processed) return;

		if (other.gameObject.name == "TrashChute")
		{
			if (TrashThrownAway != null)
				TrashThrownAway(this);

			processed = true;
			completedSound.Play();
		}
	}
}
