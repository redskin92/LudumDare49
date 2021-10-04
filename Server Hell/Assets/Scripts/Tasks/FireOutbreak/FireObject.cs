using System;
using System.Collections.Generic;
using UnityEngine;

public class FireObject : MonoBehaviour
{
	[SerializeField] private List<ParticleSystem> particles;
    [SerializeField] private AdjustedAudioSource steamSource;

	private bool active = true;
	public event Action<FireObject> FireExtinquishedEvent;
	
	private float timeToExtinquish = 3f;
	private float currentExtinquishTime = 0f;
    private float timeBeforeStopSound = 1f;
    private float curSteamStopTime = 0;

    private void Update()
    {
        if (active)
        {
            curSteamStopTime += Time.deltaTime;

            if (curSteamStopTime >= timeBeforeStopSound && steamSource.source.isPlaying)
                steamSource.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        steamSource.Play();
        curSteamStopTime = 0;
    }

    private void OnTriggerStay(Collider other)
	{
		if (!active || other.name != "StreamHitbox") return;

		currentExtinquishTime += Time.deltaTime;
		UpdateParticles();

        if (!steamSource.source.isPlaying)
            steamSource.Play();

        curSteamStopTime = 0;
	}

	private void UpdateParticles()
	{
		if(currentExtinquishTime >= timeToExtinquish)
		{
			active = false;
            steamSource.Stop();
			foreach (var particleSys in particles)
				particleSys.Stop();

			var handler = FireExtinquishedEvent;
			if (handler != null)
				FireExtinquishedEvent(this);
		}
	}
}
