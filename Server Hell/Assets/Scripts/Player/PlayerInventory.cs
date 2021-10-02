using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
	[SerializeField] private Transform EquipLocation;
	public InputAction interactAction;

	private EquipInteractable currentlyEquipedItem;
	private Transform equipedItemPrevParent;
	private bool Busy = false;

	private void Awake()
	{
		interactAction.started += InteractAction_started;
	}

	private void OnEnable()
	{
		interactAction.Enable();
	}

	private void OnDisable()
	{
		interactAction.Disable();
	}

	private void InteractAction_started(InputAction.CallbackContext obj)
	{
		if (currentlyEquipedItem != null)
			currentlyEquipedItem.ItemUse(interactAction);
		else
			Debug.Log("No targets!");
	}

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
