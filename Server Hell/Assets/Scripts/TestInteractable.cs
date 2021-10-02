using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public string interactionName = "Use Test Object";

    public string InteractionName => interactionName;

    public void Interact()
    {
        Debug.Log("Interaction detected!  Hey there :)");
    }
}
