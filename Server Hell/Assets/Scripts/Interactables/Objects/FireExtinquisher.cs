using UnityEngine;
using UnityEngine.InputSystem;

public class FireExtinquisher : EquipInteractable
{
	[SerializeField] private ParticleSystem whiteShitThatComesout;
	[SerializeField] private BoxCollider streamCollider;

	protected override void Awake()
	{
		base.Awake();
		whiteShitThatComesout.Stop();
	}

	public override void ItemUsePressed(InputAction action)
	{
		base.ItemUsePressed(action);

		whiteShitThatComesout.Play();
		streamCollider.gameObject.SetActive(true);
	}

	public override void ItemUseReleased(InputAction action)
	{
		base.ItemUseReleased(action);

		whiteShitThatComesout.Stop();
		streamCollider.gameObject.SetActive(false);
	}
}
