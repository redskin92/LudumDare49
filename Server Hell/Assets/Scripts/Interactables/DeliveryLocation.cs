using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryLocation : MonoBehaviour
{
    [SerializeField]
    protected GameObject minimapIndicator;

    protected void Awake()
    {
        minimapIndicator.SetActive(false);
    }

    public void EnableMinimapIndicator(bool enable)
    {
        minimapIndicator.SetActive(enable);
    }
}
