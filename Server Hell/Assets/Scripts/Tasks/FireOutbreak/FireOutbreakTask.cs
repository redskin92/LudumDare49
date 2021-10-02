using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOutbreakTask : UrgentTaskBase
{
	[SerializeField] private FireObject firePefab;
	[SerializeField] private List<Transform> fireSpawnLocations;

	[SerializeField] private int minFireSpawnInterval = 20;
	[SerializeField] private int maxFireSpawnInterval = 40;

	private float countUntilNextFire;

	private List<Transform> availableSpawnLocations;
	private Dictionary<FireObject, Transform> activeFires;

	private System.Random random = new System.Random();
	

	public override void ActivateTask()
	{
		availableSpawnLocations = fireSpawnLocations;
		SpawnFire();
		completed = false;
	}

	private void SpawnFire()
	{

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
		if (completed) return;

		countUntilNextFire -= Time.deltaTime;

		if(countUntilNextFire <= 0f)
		{
			countUntilNextFire = (float)random.Next(minFireSpawnInterval, maxFireSpawnInterval);
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
		if (activeFires.Count < 0)
			FireTaskComplete();
	}
}
