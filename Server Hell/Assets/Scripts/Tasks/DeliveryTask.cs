using System;
using System.Collections;
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



    private Transform current_SpawnLocation;
    private Transform current_Destination;

    private Random rand;
    private GameObject parcel;
    private GameObject landing;



    // Start is called before the first frame update
    void Start()
    {
        rand = new Random();
      
        
        Spawn();
        
    }
    
    public override void ActivateTask()
    {
       /*
        Spawn();
        base.ActivateTask();*/
    }

    void Spawn()
    {
        current_SpawnLocation = object_SpawnLocations[rand.Next(object_SpawnLocations.Count)];
        current_Destination = object_Destinations[rand.Next(object_Destinations.Count)];
        
        if (objectToDeliver != null)
        {

            var objParent = new GameObject("DeliverParent");
            
            objParent.transform.SetPositionAndRotation(current_SpawnLocation.position, Quaternion.identity); 
            
             parcel = Instantiate(objectToDeliver , objParent.transform);
            
            parcel.transform.SetParent(objParent.transform);

            parcel.AddComponent<DeliveryObject>();

            parcel.GetComponent<DeliveryObject>().ObjectDeliveredEvent += PackageDelivered;
            

        }

        if (desitnation_Indicator != null)
        {
            
            var DropOffParent = new GameObject("DropOffParent");
           
            DropOffParent.transform.SetPositionAndRotation(current_Destination.position, Quaternion.identity); 
            
             landing = Instantiate(desitnation_Indicator,DropOffParent.transform);
           
            landing.transform.SetParent(DropOffParent.transform);
            
            parcel.GetComponent<DeliveryObject>().dest = landing;

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
