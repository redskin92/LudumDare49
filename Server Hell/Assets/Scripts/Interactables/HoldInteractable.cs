using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoldInteractable : MonoBehaviour, IInteractable
{
    public string interactionName;
    public float timeToComplete;
    private PlayerInput playerInput;

    public string InteractionName => interactionName;

    /// <summary>
    /// Fired when the interaction begins.
    /// </summary>
    public event Action ProgressStarted;

    /// <summary>
    /// Fired when the interaction is complete.
    /// </summary>
    public event Action ProgressComplete;

    /// <summary>
    /// Fired when the user cancels the interaction.
    /// </summary>
    public event Action ProgressCanceled;

    /// <summary>
    /// The current progress as a float between 0 and 1.
    /// </summary>
    public float CurrentProgress { get; private set; }

	public bool Interactable { get; set; } = true;

	public void Interact(InputAction action)
    {
        action.canceled += Action_Canceled;

        StartCoroutine("StartInteractionSequence", action);

        CurrentProgress = 0;

        playerInput.actions.Disable();

        if (ProgressStarted != null)
            ProgressStarted();
    }

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
    }

    private IEnumerator StartInteractionSequence(InputAction action)
    {
        float time = 0;

        while (time <= timeToComplete)
        {
            time += Time.deltaTime;
            CurrentProgress = Mathf.Min(1, time / timeToComplete);
            yield return null;
        }

        action.canceled -= Action_Canceled;

        playerInput.actions.Enable();

        if (ProgressComplete != null)
            ProgressComplete();
    }

    private void Action_Canceled(InputAction.CallbackContext obj)
    {
        var action = obj.action;
        if (action != null)
            action.canceled -= Action_Canceled;

        StopCoroutine("StartInteractionSequence");

        CurrentProgress = 0;

        playerInput.actions.Enable();

        if (ProgressCanceled != null)
            ProgressCanceled();
    }
}
