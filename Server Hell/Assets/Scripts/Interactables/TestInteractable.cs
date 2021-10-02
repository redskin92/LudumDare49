using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public string interactionName = "Use Test Object";

    public string InteractionName => interactionName;

    public void Interact(InputAction action)
    {
        Debug.Log("Interaction detected!  Hey there :)");
    }
}
