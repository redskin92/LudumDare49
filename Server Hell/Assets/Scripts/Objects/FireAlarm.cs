using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAlarm : MonoBehaviour
{
	[SerializeField] private AudioSource alarmSound;
	[SerializeField] private Light light;

	private bool statusActive = false;
	private float timer;

	private void Awake()
	{
		SetAlarmStatus(false);
	}

	public void SetAlarmStatus(bool active)
	{
		statusActive = active;

		if (active)
		{
			light.enabled = true;
			alarmSound.Play();
			timer = 1f;
		}
		else
		{
			light.enabled = false;
			alarmSound.Stop();
		}
	}

    // Update is called once per frame
    void Update()
    {
		if(statusActive)
		{
			timer -= Time.deltaTime;

			if(timer <= 0)
			{
				timer = 1f;
				light.enabled = !light.enabled;
			}
		}
    }
}
