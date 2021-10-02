using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
	[SerializeField] private Transform EquipLocation;

	private EquipInteractable currentlyEquipedItem;
	private Transform equipedItemPrevParent;
	private bool Busy = false;

	public bool EquipItem(EquipInteractable itemToEquip)
	{
		if (currentlyEquipedItem != null || Busy) return false;

		equipedItemPrevParent = itemToEquip.transform.parent;
		currentlyEquipedItem = itemToEquip;
		currentlyEquipedItem.transform.parent = EquipLocation.transform;


		StartCoroutine(FlyEquipedItemToHand());
		return true;
	}

	public void DropItem()
	{
		StopAllCoroutines();

		if(currentlyEquipedItem != null)
		{
			currentlyEquipedItem.transform.SetParent(equipedItemPrevParent, true);
			currentlyEquipedItem.DropItem();

			currentlyEquipedItem = null;
			equipedItemPrevParent = null;
		}
	}

	private IEnumerator FlyEquipedItemToHand(float equipTime = 1f)
	{
		Busy = true;
		float currentTime = 1f;

		while(currentTime > 0)
		{
			yield return null;
			currentTime -= Time.deltaTime;

			currentlyEquipedItem.transform.localPosition = Vector3.Lerp(currentlyEquipedItem.transform.localPosition, Vector3.zero, equipTime - currentTime);
			currentlyEquipedItem.transform.localRotation = Quaternion.Lerp(currentlyEquipedItem.transform.localRotation, Quaternion.identity, equipTime - currentTime);
		}

		Busy = false;
	}
}
