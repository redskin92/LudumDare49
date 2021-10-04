using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DeliveryTask : TaskBase
{
    /// possible flavor stuff
    /// -- indicators for the drop off point ( flare)
    /// -- pick up sounds
    /// -- completion sound
    /// -- escalations
    /// <summary>
    /// The item being transport 
    /// </summary>
    [SerializeField]
    protected GameObject objectToDeliver;

    /// <summary>
    /// List of possible spawn location for 
    /// </summary>
    [SerializeField]
    protected List<Transform> object_SpawnLocations;

    [SerializeField]
    protected GameObject desitnation_Indicator;

    /// <summary>
    /// list of possible item drop off points 
    /// </summary>
    [SerializeField]
    protected List<Transform> object_Destinations;

    private Transform current_Destination;


    private Transform current_SpawnLocation;
    private GameObject landing;
    private GameObject parcel;

    private readonly Random rand = new Random();


    public override void ActivateTask()
    {
        Spawn();
        base.ActivateTask();
    }

    private void Spawn()
    {
        current_SpawnLocation = object_SpawnLocations[rand.Next(object_SpawnLocations.Count)];
        current_Destination = object_Destinations[rand.Next(object_Destinations.Count)];

        if (objectToDeliver != null)
        {
            GameObject objParent = new GameObject("DeliverParent");

            objParent.transform.SetPositionAndRotation(current_SpawnLocation.position, Quaternion.identity);

            parcel = Instantiate(objectToDeliver, objParent.transform);

            parcel.transform.SetParent(objParent.transform);


            DeliveryObject deliveryObject = parcel.GetComponent<DeliveryObject>();
            if(deliveryObject == null)
                parcel.AddComponent<DeliveryObject>();

            deliveryObject.ObjectDeliveredEvent += PackageDelivered;
        }

        if (desitnation_Indicator != null)
        {
            GameObject DropOffParent = new GameObject("DropOffParent");

            DropOffParent.transform.SetPositionAndRotation(current_Destination.position, Quaternion.identity);

            landing = Instantiate(desitnation_Indicator, DropOffParent.transform);

            landing.transform.SetParent(DropOffParent.transform);

            DeliveryLocation deliveryLocation = landing.GetComponent<DeliveryLocation>();
            deliveryLocation.EnableMinimapIndicator(false);

            parcel.GetComponent<DeliveryObject>().dest = landing;
            parcel.GetComponent<DeliveryObject>().SetDestination(landing);
        }
    }

    protected virtual void PackageDelivered(DeliveryObject pak)
    {
        parcel.GetComponent<DeliveryObject>().ObjectDeliveredEvent -= PackageDelivered;
        Destroy(parcel);
        Destroy(landing);
        FireTaskComplete();
    }
}