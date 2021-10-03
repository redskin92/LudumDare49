using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerTask : UrgentTaskBase
{
	[SerializeField] private HoldInteractable serverIteractable;
	[SerializeField] private float failTimer = 30f;

	[SerializeField] private List<Light> lights;

	private float lightsTimer;

	float currentTimer;

	public void Awake()
	{
		foreach (var light in lights)
			light.color = Color.green;

		lightsTimer = 1f;
		serverIteractable.Interactable = false;
		serverIteractable.ProgressComplete += ServerRepaired;

		ActivateTask();
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
		FireTaskComplete();

		foreach (var light in lights)
			light.color = Color.green;
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
}

