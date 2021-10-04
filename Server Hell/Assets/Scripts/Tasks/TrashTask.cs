using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashTask : TaskBase
{
	[SerializeField] private Trashbag trashBagPrefab;
	[SerializeField] private List<Transform> trashSpawnLocations;
	[SerializeField] private int startingBagAmount = 5;

	public AudioSource completedSound;
	private List<Trashbag> trashBags = new List<Trashbag>();


	// Start is called before the first frame update
	void Start()
    {
		UpdateTaskName("Throw away trash (0/ " + startingBagAmount + ")");

	}

	public override void ActivateTask()
	{

		List<Transform> availLocations = new List<Transform>(trashSpawnLocations);

		for (int i = 0; i < startingBagAmount; ++i)
		{
			var spawnLocation = availLocations[Random.Range(0, availLocations.Count)];
			availLocations.Remove(spawnLocation);

			var bag = GameObject.Instantiate(trashBagPrefab, spawnLocation);
			trashBags.Add(bag);
			bag.TrashThrownAway += TrashProcessed;
		}

		base.ActivateTask();
	}


	private void TrashProcessed(Trashbag bag)
	{
		bag.TrashThrownAway -= TrashProcessed;

		if(trashBags.Contains(bag))
			trashBags.Remove(bag);

		UpdateTaskName("Throw away trash (" + (startingBagAmount - trashBags.Count) + " / " + startingBagAmount + ")");


		Destroy(bag.gameObject);
		completedSound.Play();

		if (trashBags.Count <= 0)
		{
			FireTaskComplete();
		}
	}
}
