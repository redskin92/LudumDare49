using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
	[SerializeField] private Transform EquipLocation;
	public InputAction itemUseAction;
	public InputAction itemDropAction;


	private EquipInteractable currentlyEquipedItem;
	private Transform equipedItemPrevParent;
	private bool Busy = false;

	private void Awake()
	{
		itemUseAction.started += ItemUseAction;
		itemDropAction.started += ItemDropAction;
	}

	private void ItemUseAction(InputAction.CallbackContext obj)
	{
		if (currentlyEquipedItem == null) return;

		currentlyEquipedItem.ItemUse(itemUseAction);
	}

	private void ItemDropAction(InputAction.CallbackContext obj)
	{
		if (currentlyEquipedItem == null) return;

		itemUseAction.Disable();
		itemDropAction.Disable();
		DropItem();
	}

	public bool EquipItem(EquipInteractable itemToEquip)
	{
		if (currentlyEquipedItem != null || Busy) return false;

		itemUseAction.Enable();
		itemDropAction.Enable();

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
