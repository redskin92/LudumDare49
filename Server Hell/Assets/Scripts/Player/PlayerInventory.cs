using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
	[SerializeField] private PlayerInteract playerInteract;
	[SerializeField] private Transform EquipLocation;
	public InputAction itemUseAction;
	public InputAction itemDropAction;

	private string dropAction = "Press Q to drop";

	private EquipInteractable currentlyEquipedItem;
	private Transform equipedItemPrevParent;
	private bool Busy = false;

	private TextMeshProUGUI interactionTextMesh => playerInteract.interactionTextMesh;

	private void Awake()
	{
		itemUseAction.started += ItemUseAction;
		itemUseAction.canceled += ItemUseCancelled;

		itemDropAction.started += ItemDropAction;
	}

	private void ItemUseAction(InputAction.CallbackContext obj)
	{
		if (currentlyEquipedItem == null) return;
		currentlyEquipedItem.ItemUsePressed(itemUseAction);
	}

	private void ItemUseCancelled(InputAction.CallbackContext obj)
	{
		if (currentlyEquipedItem == null) return;
		currentlyEquipedItem.ItemUseReleased(itemUseAction);
	}

	private void ItemDropAction(InputAction.CallbackContext obj)
	{
		if (currentlyEquipedItem == null) return;
		DropItem();
	}

	public bool EquipItem(EquipInteractable itemToEquip)
	{
		if (currentlyEquipedItem != null || Busy) return false;

		playerInteract.enabled = false;

		interactionTextMesh.text = dropAction + '\n' + itemToEquip.useActionName;

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

		playerInteract.enabled = true;

		itemUseAction.Disable();
		itemDropAction.Disable();

		if (currentlyEquipedItem != null)
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
