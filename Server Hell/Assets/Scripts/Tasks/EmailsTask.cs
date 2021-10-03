using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailsTask : TaskBase
{
	[SerializeField] private HoldInteractable computerHold;
	[SerializeField] private Material defaultScreenMaterial;
	[SerializeField] private Material taskActiveScreenMaterial;


	private void Awake()
	{
		computerHold.Interactable = false;
		UpdateMaterial(false);
	}

	public override void ActivateTask()
	{
		base.ActivateTask();

		computerHold.Interactable = true;
		computerHold.ProgressComplete += ComputerHold_ProgressComplete;

		UpdateMaterial(true);
	}

	private void ComputerHold_ProgressComplete()
	{
		computerHold.ProgressComplete -= ComputerHold_ProgressComplete;
		computerHold.Interactable = false;
		FireTaskComplete();

		UpdateMaterial(false);
	}

	private void UpdateMaterial(bool active)
	{
		var meshRenderer = computerHold.GetComponent<MeshRenderer>();

		var materials = meshRenderer.materials;
		materials[1] = active ? taskActiveScreenMaterial : defaultScreenMaterial;

		meshRenderer.materials = materials; 
	}
}
