using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashTask : TaskBase
{
	[SerializeField] private List<Trashbag> trashBags;
	
    // Start is called before the first frame update
    void Start()
    {
        foreach(var bag in trashBags)
		{
			bag.TrashThrownAway += TrashProcessed;
		}
    }

	private void TrashProcessed(Trashbag bag)
	{
		bag.TrashThrownAway -= TrashProcessed;

		if(trashBags.Contains(bag))
			trashBags.Remove(bag);

		if (trashBags.Count <= 0)
		{
			FireTaskComplete();
		}
	}
}
