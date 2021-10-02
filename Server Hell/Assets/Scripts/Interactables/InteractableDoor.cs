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

	public bool Interactable { get; set; } = true;

	#endregion

	#region Private Fields

	private bool doorOpenedStatus = false;
    
    public string interactionName = "Open/Close A Door";

    [SerializeField]
    protected Vector3 defaultRotation;
    
    [SerializeField]
    protected Vector3 targetRotation;
    #endregion

  
    public string InteractionName => interactionName;

    public void Interact(InputAction action)
    {
		if (!Interactable) return;

        doorOpenedStatus = !doorOpenedStatus;
        
        if(!doorOpenedStatus)
            StartCoroutine(RotateDoor(pivot.transform.localEulerAngles + targetRotation));
        else
            StartCoroutine(RotateDoor(pivot.transform.localEulerAngles + defaultRotation));
    }

    private IEnumerator RotateDoor(Vector3 rot)
    {
        float counter = 0;

        Vector3 angle = pivot.transform.eulerAngles;
        
        while (counter < duration)
        { 
            counter += Time.deltaTime;
            pivot.transform.localEulerAngles = Vector3.Lerp(angle, rot, counter / duration);
            yield return null;
        }
    }
}
