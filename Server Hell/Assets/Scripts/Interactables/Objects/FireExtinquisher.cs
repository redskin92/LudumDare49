using UnityEngine;
using UnityEngine.InputSystem;

public class FireExtinquisher : EquipInteractable
{
	[SerializeField] private ParticleSystem whiteShitThatComesout;

	protected override void Awake()
	{
		base.Awake();
		whiteShitThatComesout.Stop();
	}

	public override void ItemUse(InputAction action)
	{
		base.ItemUse(action);

		whiteShitThatComesout.Play();
	}
}
