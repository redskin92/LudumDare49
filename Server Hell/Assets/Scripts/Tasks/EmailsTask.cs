using System.Collections.Generic;
using UnityEngine;

public class EmailsTask : TaskBase
{
	[SerializeField] private HoldInteractable computerHold;
	[SerializeField] private Material screenMaterial;

	[SerializeField]
	private List<Texture2D> backgroundTextures;
	[SerializeField]
	private Texture2D emailTexture;

    private ProgressMeter progressMeter;
    private MeshRenderer meshRenderer;

    private bool interactionStarted;

	System.Random random = new System.Random();

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

        computerHold.ProgressStarted += ComputerHold_ProgressStarted;
        computerHold.ProgressComplete += ComputerHold_ProgressComplete;
        computerHold.ProgressCanceled += ComputerHold_ProgressCanceled;
    }

    private void Start()
    {
        progressMeter = FindObjectOfType<ProgressMeter>();
    }

    private void OnDestroy()
    {
        if (computerHold != null)
        {
            computerHold.ProgressStarted -= ComputerHold_ProgressStarted;
            computerHold.ProgressComplete -= ComputerHold_ProgressComplete;
            computerHold.ProgressCanceled -= ComputerHold_ProgressCanceled;
        }
    }

    private void Update()
    {
        if (interactionStarted)
            progressMeter.SetProgress(computerHold.CurrentProgress);
    }

    public override void ActivateTask()
	{
		base.ActivateTask();

		computerHold.Interactable = true;

		UpdateTexture(true);
	}

    private void ComputerHold_ProgressStarted()
    {
        interactionStarted = true;
    }

    private void ComputerHold_ProgressComplete()
	{
        interactionStarted = false;
        computerHold.ProgressComplete -= ComputerHold_ProgressComplete;
		computerHold.Interactable = false;
        progressMeter.ProgressComplete();
		FireTaskComplete();

		UpdateTexture(false);
    }

    private void ComputerHold_ProgressCanceled()
    {
        interactionStarted = false;

        progressMeter.ResetProgress();
    }

    private void UpdateTexture(bool active)
	{
		ToggleMonitor(true);
		meshRenderer.materials[1].SetTexture("_EmissionTexture", active ? emailTexture : backgroundTextures[random.Next(backgroundTextures.Count)]);
	}

	private void ToggleMonitor(bool enable)
	{
		meshRenderer.materials[1].SetFloat("_EmissionAmount", enable ? 1.0f : 0.0f);
	}
}
