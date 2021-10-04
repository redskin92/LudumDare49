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
            parcel = Instantiate(objectToDeliver, transform);

            DeliveryObject deliveryObject = parcel.GetComponent<DeliveryObject>();
            if(deliveryObject == null)
                parcel.AddComponent<DeliveryObject>();

            deliveryObject.ObjectDeliveredEvent += PackageDelivered;
        }

        if (desitnation_Indicator != null)
        {
            landing = Instantiate(desitnation_Indicator, transform);

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