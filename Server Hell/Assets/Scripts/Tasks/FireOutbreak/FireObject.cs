using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObject : MonoBehaviour
{
	[SerializeField] private List<ParticleSystem> particles;

	private bool active = true;
	public event Action<FireObject> FireExtinquishedEvent;
	
	private float timeToExtinquish = 3f;
	private float currentExtinquishTime = 0f;

	private void OnTriggerStay(Collider other)
	{
		if (!active || other.name != "StreamHitbox") return;

		currentExtinquishTime += Time.deltaTime;
		UpdateParticles();
	}

	private void UpdateParticles()
	{
		if(currentExtinquishTime >= timeToExtinquish)
		{
			active = false;
			foreach (var particleSys in particles)
				particleSys.Stop();

			var handler = FireExtinquishedEvent;
			if (handler != null)
				FireExtinquishedEvent(this);
		}
	}
}
