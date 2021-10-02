using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerTask : UrgentTaskBase
{
	[SerializeField] private HoldInteractable serverIteractable;
	[SerializeField] private float failTimer = 30f;

	float currentTimer;

	public void Awake()
	{
		serverIteractable.Interactable = false;
		serverIteractable.ProgressComplete += ServerRepaired;	
	}

	public override void ActivateTask()
	{
		base.ActivateTask();
		currentTimer = failTimer;

		serverIteractable.Interactable = true;
	}

	private void ServerRepaired()
	{
		IsActive = false;
		serverIteractable.Interactable = false;
		FireTaskComplete();
	}

	public void Update()
	{
		if (!IsActive) return;

		currentTimer -= Time.deltaTime;

		if(currentTimer <= 0)
		{
			currentTimer = failTimer;
			FireTaskFailed();
		}
	}

}

