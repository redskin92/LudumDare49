using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailsTask : TaskBase
{
	[SerializeField] private HoldInteractable computerHold;
	[SerializeField] private Material screenMaterial;

	[SerializeField]
	private Texture2D backgroundTexture;
	[SerializeField]
	private Texture2D emailTexture;

	private MeshRenderer meshRenderer;

	private void Awake()
	{
		computerHold.Interactable = false;
		meshRenderer = computerHold.GetComponent<MeshRenderer>();
		var materials = meshRenderer.materials;
		materials[1] = screenMaterial;
		meshRenderer.materials = materials;
		UpdateTexture(false);

		float rng = Random.Range(0, 100f);
		ToggleMonitor(rng > 50f);

		rng = Random.Range(0, 100f);
		if(rng > 50f)
			UpdateTexture(false);
	}

	public override void ActivateTask()
	{
		base.ActivateTask();

		computerHold.Interactable = true;
		computerHold.ProgressComplete += ComputerHold_ProgressComplete;

		UpdateTexture(true);
	}

	private void ComputerHold_ProgressComplete()
	{
		computerHold.ProgressComplete -= ComputerHold_ProgressComplete;
		computerHold.Interactable = false;
		FireTaskComplete();

		UpdateTexture(false);
	}

	private void UpdateTexture(bool active)
	{
		ToggleMonitor(true);
		meshRenderer.materials[1].SetTexture("_EmissionTexture", active ? emailTexture : backgroundTexture);
	}

	private void ToggleMonitor(bool enable)
	{
		meshRenderer.materials[1].SetFloat("_EmissionAmount", enable ? 1.0f : 0.0f);
	}
}
