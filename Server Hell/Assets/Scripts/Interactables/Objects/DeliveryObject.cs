using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryObject : EquipInteractable
{
    public override string useActionName => "Deliver Item";

    public GameObject dest;
    public event Action<DeliveryObject> ObjectDeliveredEvent;
    
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
