using UnityEngine;

public class ServerLight : MonoBehaviour
{
	private Light light;

	private float minCdTime = 0.05f;
	private float maxCdTime = 0.35f;
	private float currentCD;

	private bool stableStatus = true;

    void Awake()
    {
		currentCD = GetTimeRoll();

		light = GetComponent<Light>();
    }

	public void SetLightStatus(bool statusOK)
	{
		if (stableStatus == statusOK) return;

		stableStatus = statusOK;

		if(statusOK)
		{
			currentCD = GetTimeRoll();
			light.color = Color.green;
		}
		else
		{
			light.color = Color.red;
			light.enabled = true;
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (!stableStatus) return;

		currentCD -= Time.deltaTime;
		if(currentCD <= 0)
		{
			currentCD = GetTimeRoll();
			light.enabled = !light.enabled;
		}
    }

	private float GetTimeRoll()
	{
		return Random.Range(minCdTime, maxCdTime);
	}
}
