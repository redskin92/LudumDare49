using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashTask : TaskBase
{
	[SerializeField] private List<Trashbag> trashBags;
	public AudioSource completedSound;

	private int startingbagamount; 


	// Start is called before the first frame update
	void Start()
    {
        foreach(var bag in trashBags)
		{
			bag.TrashThrownAway += TrashProcessed;
		}

		startingbagamount = trashBags.Count;
		UpdateTaskName("Throw away trash (" + (startingbagamount - trashBags.Count) + " / " + startingbagamount + ")");
	}

	private void TrashProcessed(Trashbag bag)
	{
		bag.TrashThrownAway -= TrashProcessed;

		UpdateTaskName("Take out trash (" + (startingbagamount - trashBags.Count) + " / " + startingbagamount + ")");

		if(trashBags.Contains(bag))
			trashBags.Remove(bag);

		Destroy(bag.gameObject);
		completedSound.Play();

		if (trashBags.Count <= 0)
		{
			FireTaskComplete();
		}
	}
}
