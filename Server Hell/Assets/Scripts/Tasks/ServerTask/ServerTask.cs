using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerTask : UrgentTaskBase
{
	[SerializeField] private HoldInteractable serverIteractable;
	[SerializeField] private float failTimer = 30f;

	[SerializeField] private List<Light> lights;

    private ProgressMeter progressMeter;

	private float lightsTimer;

	float currentTimer;

    private bool interactionStarted;

    public void Awake()
	{
		foreach (var light in lights)
			light.color = Color.green;

		lightsTimer = 1f;
        serverIteractable.ProgressStarted += ServerIteractable_ProgressStarted;
        serverIteractable.ProgressComplete += ServerRepaired;
        serverIteractable.ProgressCanceled += ServerIteractable_ProgressCanceled;
    }

    private void OnDestroy()
    {
        if (serverIteractable != null)
        {
            serverIteractable.ProgressStarted += ServerIteractable_ProgressStarted;
            serverIteractable.ProgressComplete += ServerRepaired;
            serverIteractable.ProgressCanceled += ServerIteractable_ProgressCanceled;
        }
    }

    public override void ActivateTask()
	{
		base.ActivateTask();
		currentTimer = failTimer;

		foreach (var light in lights)
		{
			light.color = Color.red;
			light.enabled = true;
		}

		lightsTimer = 0.5f;

		serverIteractable.Interactable = true;
	}

	private void ServerRepaired()
	{
		IsActive = false;
		serverIteractable.Interactable = false;
        interactionStarted = false;
        progressMeter.ProgressComplete();
		FireTaskComplete();

		foreach (var light in lights)
			light.color = Color.green;
	}

    private void Start()
    {
        progressMeter = FindObjectOfType<ProgressMeter>();
    }

    public void Update()
	{
		UpdateLights();

		if (!IsActive) return;

		currentTimer -= Time.deltaTime;

		if (currentTimer <= 0)
		{
			currentTimer = failTimer;
			FireTaskFailed();
		}

        if (interactionStarted)
            progressMeter.SetProgress(serverIteractable.CurrentProgress);
    }

	private void UpdateLights()
	{
		lightsTimer -= Time.deltaTime;

		if(lightsTimer <= 0f)
		{
			foreach (var light in lights)
				light.enabled = !light.enabled;

			lightsTimer = IsActive ? 0.5f : 2f;
		}
    }

    private void ServerIteractable_ProgressStarted()
    {
        interactionStarted = true;
    }

    private void ServerIteractable_ProgressCanceled()
    {
        interactionStarted = false;

        progressMeter.ResetProgress();
    }
}

