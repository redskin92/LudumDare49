using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeliveryObject : EquipInteractable
{
    public override string useActionName => "Deliver Item";

    public GameObject dest;
    public event Action<DeliveryObject> ObjectDeliveredEvent;

	[SerializeField] private GameObject minimapIndicator;
	[SerializeField] private AudioSource successAudio;

	protected DeliveryLocation deliveryLocation;

    public void SetDestination(GameObject destination)
    {
        dest = destination;
        deliveryLocation = dest.GetComponent<DeliveryLocation>();
    }
    
    public override void DropItem(Vector3 force)
    {
        base.DropItem(force);
		minimapIndicator.SetActive(true);
		deliveryLocation.EnableIndicators(false);
    }

    public override void Interact(InputAction action)
    {
        base.Interact(action);

		minimapIndicator.SetActive(false);
		deliveryLocation.EnableIndicators(true);
    }
    
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != dest) return;

		successAudio.Play();

		dest = null;
		minimapIndicator.SetActive(false);
			   
		var done = ObjectDeliveredEvent;

        if (done != null)
            ObjectDeliveredEvent(this);

    }
}
