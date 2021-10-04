using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeliveryObject : EquipInteractable
{
    public override string useActionName => "Deliver Item";

    public GameObject dest;
    public event Action<DeliveryObject> ObjectDeliveredEvent;

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
        
        deliveryLocation.EnableIndicators(false);
    }

    public override void Interact(InputAction action)
    {
        base.Interact(action);

        deliveryLocation.EnableIndicators(true);
    }
    
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != dest) return;

		successAudio.Play();

		dest = null;

		var done = ObjectDeliveredEvent;

        if (done != null)
            ObjectDeliveredEvent(this);

    }
}
