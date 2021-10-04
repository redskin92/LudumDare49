using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeliveryObject : EquipInteractable
{
    public override string useActionName => "Deliver Item";

    public GameObject dest;
    public event Action<DeliveryObject> ObjectDeliveredEvent;

    protected DeliveryLocation deliveryLocation;

    public void SetDestination(GameObject destination)
    {
        Debug.Log(string.Format("DeliveryObject::SetDestination Line(18) - destination: {0}", destination, this));
        dest = destination;
        deliveryLocation = dest.GetComponent<DeliveryLocation>();
    }
    
    public override void DropItem(Vector3 force)
    {
        base.DropItem(force);
        
        deliveryLocation.EnableMinimapIndicator(false);
    }

    public override void Interact(InputAction action)
    {
        base.Interact(action);

        deliveryLocation.EnableMinimapIndicator(true);
    }
    
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != dest) return;
        
        Debug.Log("Firing the OBject Delivered Event ");
        var done = ObjectDeliveredEvent;

        if (done != null)
            ObjectDeliveredEvent(this);

    }
}
