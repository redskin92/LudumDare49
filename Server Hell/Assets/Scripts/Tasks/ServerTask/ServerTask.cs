using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServerTask : UrgentTaskBase
{
	[SerializeField] private HoldInteractable serverIteractable;
	[SerializeField] private float failTimer = 30f;

	[SerializeField] private Transform slotParent;
	[SerializeField] private GameObject slotFan, slotScreen;

	[SerializeField]
	private GameObject minimapIndicator;

	private List<ServerLight> lights = new List<ServerLight>();
    private ProgressMeter progressMeter;
    private bool interactionStarted;

	private void Start()
	{
		progressMeter = FindObjectOfType<ProgressMeter>();
		minimapIndicator.SetActive(false);
	}

	public void Awake()
	{
		PopulateSlots();

		foreach (var light in lights)
			light.SetLightStatus(true);

		serverIteractable.Interactable = false;
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

	private void PopulateSlots()
	{
		int numSlots = 8;
		List<GameObject> slotsToAdd = new List<GameObject>();

		slotsToAdd.Add(slotScreen);
		while(slotsToAdd.Count < numSlots)
		{
			var slotType = Random.Range(0, 2) == 0 ? slotFan : slotScreen;
			slotsToAdd.Add(Random.Range(0, 7) == 0 ? null : slotType);
		}

		slotsToAdd = slotsToAdd.OrderBy(c => Random.Range(0, 100)).ToList();


		float yPosition = 2.6f;
		for(int i = 0; i < 8; ++i)
		{
			if (slotsToAdd[i] != null)
			{
				var slot = GameObject.Instantiate(slotsToAdd[i], slotParent);
				slot.transform.localPosition = new Vector3(0.0f, yPosition, 0.0f);
				lights.AddRange(slot.GetComponentsInChildren<ServerLight>());
			}
			yPosition -= 0.45f;
		}
	}

    public override void ActivateTask()
	{
		base.ActivateTask();

		foreach (var light in lights)
			light.SetLightStatus(false);
		
		minimapIndicator.SetActive(true);

		serverIteractable.Interactable = true;
	}

	private void ServerRepaired()
	{
		IsActive = false;
		serverIteractable.Interactable = false;
        interactionStarted = false;
        progressMeter.ProgressComplete();
		FireTaskComplete();
		
		minimapIndicator.SetActive(false);

		foreach (var light in lights)
			light.SetLightStatus(true);
	}



    public void Update()
	{
		if (!IsActive) return;

        if (interactionStarted)
            progressMeter.SetProgress(serverIteractable.CurrentProgress);
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

