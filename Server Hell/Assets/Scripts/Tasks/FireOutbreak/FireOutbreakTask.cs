using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireOutbreakTask : UrgentTaskBase
{
	[SerializeField] private FireObject firePefab;
	[SerializeField] private List<Transform> fireSpawnLocations;
	[SerializeField] private FireAlarm fireAlarm;

	[SerializeField] private int minFireSpawnInterval = 20;
	[SerializeField] private int maxFireSpawnInterval = 40;

	[SerializeField] private List<FireExtinquisherMapIndicator> extinquishers;

	private float countUntilNextFire;

	private List<Transform> availableSpawnLocations;
	private Dictionary<FireObject, Transform> activeFires = new Dictionary<FireObject, Transform>();

	private System.Random random = new System.Random();

	public override void ActivateTask()
	{
		activeFires.Clear();
		availableSpawnLocations = new List<Transform>(fireSpawnLocations);
		SpawnFire();
		fireAlarm.SetAlarmStatus(true);
		base.ActivateTask();


		foreach (var ext in extinquishers)
			ext.gameObject.SetActive(true);
	}

	private void SpawnFire()
	{
		countUntilNextFire = (float)random.Next(minFireSpawnInterval, maxFireSpawnInterval);

		if (availableSpawnLocations.Count <= 0)
		{
			FireTaskFailed();
			return;
		}

		int randomLocationIndex = random.Next(availableSpawnLocations.Count);
		var spawnLocation = availableSpawnLocations[randomLocationIndex];

		availableSpawnLocations.Remove(spawnLocation);

		var newFire = GameObject.Instantiate(firePefab, spawnLocation.transform);
		activeFires.Add(newFire, spawnLocation);
		newFire.FireExtinquishedEvent += FireExtinquished;
	}

	public void Update()
	{
		if (!IsActive) return;

		countUntilNextFire -= Time.deltaTime;

		if(countUntilNextFire <= 0f)
		{
			SpawnFire();
		}
	}

	private void FireExtinquished(FireObject fire)
	{
		if (!activeFires.ContainsKey(fire))
			Debug.LogError("Trying to find fire that isn't in the held objects");

		var spawnLocation = activeFires[fire];
		availableSpawnLocations.Add(spawnLocation);

		activeFires.Remove(fire);

		Destroy(fire.gameObject);
		CheckForTaskComplete();
	}

	private void CheckForTaskComplete()
	{
		if (activeFires.Count <= 0)
		{
			fireAlarm.SetAlarmStatus(false);


			// If there are no other active fires - turn off the indicators.
			if (TaskManager.Instance.urgentTaskGroup.Count(x => x.task.taskName == this.taskName && x.task.IsActive) <= 1)
			{
				foreach (var ext in extinquishers)
					ext.gameObject.SetActive(false);
			}

			FireTaskComplete();
		}
	}
}
