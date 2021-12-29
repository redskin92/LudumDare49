using UnityEngine;

public class FireExtinquisherMapIndicator : MonoBehaviour
{
	[SerializeField] private Quaternion orientation;

	bool scaleUp = false;
	float scaleCurr = 1f;

	private void Start()
	{
		this.gameObject.SetActive(false);
	}

	void Update()
    {
		this.transform.rotation = orientation;


		float increment = Time.deltaTime * (scaleUp ? 1f : -1f);
		scaleCurr += increment;


		if (scaleCurr < 0.5f)
		{
			scaleCurr = 0.5f;
			scaleUp = true;
		}
		else if(scaleCurr > 1f)
		{
			scaleUp = false;
			scaleCurr = 1f;
		}

		this.transform.localScale = new Vector3(scaleCurr, scaleCurr, scaleCurr);
    }
}
