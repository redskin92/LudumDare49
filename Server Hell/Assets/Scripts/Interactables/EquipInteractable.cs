using UnityEngine;
using UnityEngine.InputSystem;

public class EquipInteractable : MonoBehaviour, IInteractable
{
    public string interactionName = "Press 'E' to Equip";
    private Collider objectCollider;

    private Rigidbody rbody;
    public virtual string useActionName => string.Empty;


    protected virtual void Awake()
    {
        rbody = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
    }

    public string InteractionName => interactionName;

    public bool Interactable { get; set; } = true;

    public virtual void Interact(InputAction action)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        if (inventory.EquipItem(this))
        {
            objectCollider.enabled = false;
            rbody.isKinematic = true;
        }
    }

    public virtual void DropItem(Vector3 force)
    {
        objectCollider.enabled = true;
        rbody.isKinematic = false;

        if (force != Vector3.zero)
            rbody.AddForce(force, ForceMode.Impulse);
    }


    public virtual void ItemUsePressed(InputAction action) { }

    public virtual void ItemUseReleased(InputAction action) { }
}