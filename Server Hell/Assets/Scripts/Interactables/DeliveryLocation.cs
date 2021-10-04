using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryLocation : MonoBehaviour
{
    [SerializeField]
    protected GameObject minimapIndicator;

	[SerializeField] private GameObject arrowIndicator;

    protected void Awake()
    {
        minimapIndicator.SetActive(false);
		arrowIndicator.SetActive(false);
    }

    public void EnableIndicators(bool enable)
    {
        minimapIndicator.SetActive(enable);
		arrowIndicator.SetActive(enable);
    }
}
