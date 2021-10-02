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
        RaycastHit[] hits = Physics.RaycastAll(cameraT.position, cameraT.forward, interactDistance);

        curInteractable = FindInteractable(hits);

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

    /// <summary>
    /// Find the first interactable object in the list of things hit.
    /// </summary>
    /// <param name="hits">Array of raycasted objects.</param>
    /// <returns>The first interactable object raycasted, or null if not found.</returns>
    private IInteractable FindInteractable(RaycastHit[] hits)
    {
        IInteractable interactable = null;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.tag == "Interactable")
            {
                var comp = hits[i].transform.GetComponent<IInteractable>();
                if (comp != null && comp.Interactable)
                {
                    interactable = comp;
                    break;
                }
            }
        }

        return interactable;
    }

    private void InteractAction_started(InputAction.CallbackContext obj)
    {
        Interact();
    }
}
