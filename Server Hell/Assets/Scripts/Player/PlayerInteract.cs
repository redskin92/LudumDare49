using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// Handles simple player interactions with objects.
/// </summary>
public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 5f;

    /// <summary>
    /// The text mesh that displays "Press 'E' to...".
    /// </summary>
    public TextMeshProUGUI interactionTextMesh;

    public Transform cameraT;

    public InputAction interactAction;

    /// <summary>
    /// The object we would interact with if we press the
    /// Interact key.
    /// </summary>
    private IInteractable curInteractable;

    private void Awake()
    {
        interactAction.started += InteractAction_started;
    }

    private void OnEnable()
    {
        interactAction.Enable();
    }

    private void OnDisable()
    {
        interactAction.Disable();
    }

    private void Update()
    {
        RaycastHit info;
        curInteractable = null;
        if (Physics.Raycast(cameraT.position, cameraT.forward, out info, interactDistance))
        {
            if (info.collider.transform.tag == "Interactable")
            {
                var comp = info.collider.GetComponent<IInteractable>();
                if (comp != null && comp.Interactable)
                    curInteractable = comp;
            }
        }

        if (curInteractable != null)
        {
            interactionTextMesh.text = curInteractable.InteractionName;
            interactionTextMesh.gameObject.SetActive(true);
        }
        else
        {
            interactionTextMesh.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(cameraT.position, cameraT.forward * interactDistance);
    }

    private void Interact()
    {
        if (curInteractable != null)
            curInteractable.Interact(interactAction);
        else
            Debug.Log("No targets!");
    }

    private void InteractAction_started(InputAction.CallbackContext obj)
    {
        Interact();
    }
}
