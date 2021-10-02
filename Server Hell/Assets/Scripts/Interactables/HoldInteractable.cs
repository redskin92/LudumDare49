using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoldInteractable : MonoBehaviour, IInteractable
{
    public string interactionName;
    public float timeToComplete;

    public string InteractionName => interactionName;

    public event Action ProgressComplete;

    public void Interact(InputAction action)
    {
        action.canceled += Action_Canceled;

        StartCoroutine("StartInteractionSequence");
    }

    private IEnumerator StartInteractionSequence()
    {
        float time = 0;

        while (time <= timeToComplete)
        {
            time += Time.deltaTime;
            yield return null;
        }

        if (ProgressComplete != null)
            ProgressComplete();
    }

    private void Action_Canceled(InputAction.CallbackContext obj)
    {
        var action = obj.action;
        if (action != null)
            action.canceled -= Action_Canceled;

        StopCoroutine("StartInteractionSequence");
    }
}
