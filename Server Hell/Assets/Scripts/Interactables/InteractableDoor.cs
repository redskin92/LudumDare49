using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InteractableDoor : MonoBehaviour, IInteractable
{


    #region Public Fields

    /// <summary>
    /// Object to rotate the door to open and close on 
    /// </summary>
    [SerializeField]
    protected GameObject pivot;

    [SerializeField]
    protected float rotatationSpeed = 0.0f;
    
    [SerializeField]
    protected AudioClip doorSounds;

    [SerializeField]
    protected float duration;
    
    #endregion

    #region Private Fields

    private bool doorOpenedStatus = false;
    
    public string interactionName = "Open/Close A Door";

    [SerializeField]
    protected Vector3 defaultRotatation;
    
    [SerializeField]
    protected Vector3 targetRotatation;
    #endregion

    private void Start()
    {
       // defaultPivot = pivot.transform.eulerAngles;
    }

    public string InteractionName => interactionName;

    public void Interact(InputAction action)
    {
        doorOpenedStatus = !doorOpenedStatus;
        
        if(!doorOpenedStatus)
            StartCoroutine(RotateDoor(pivot.transform.eulerAngles + targetRotatation));
        else
            StartCoroutine(RotateDoor(defaultRotatation));
    }

    private IEnumerator RotateDoor(Vector3 rot)
    {
        float counter = 0;

        Vector3 angle ;
        
        angle = pivot.transform.eulerAngles;
        
        while (counter < duration)
        { 
            counter += Time.deltaTime;
            pivot.transform.eulerAngles = Vector3.Lerp(angle, rot, counter / duration);
            yield return null;
        }
    }
}
