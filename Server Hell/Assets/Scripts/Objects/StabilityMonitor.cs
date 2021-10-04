using System;
using UnityEngine;
using TMPro;

public class StabilityMonitor : MonoBehaviour
{
    private const string STABILITY_FORMAT = "{0}%";

    [SerializeField]
    private TextMeshPro textMesh;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (StabilityMeter.Instance != null)
        {
            StabilityMeter.Instance.StabilityUpdated += StabilityMeter_StabilityUpdated;
            StabilityMeter.Instance.MinStabilityReached += StabilityMeter_MinStabilityReady;
        }
    }

    private void UpdateStabilityText(int stability)
    {
        textMesh.text = string.Format(STABILITY_FORMAT, stability);
    }

    private void StabilityMeter_StabilityUpdated()
    {
        int stability = StabilityMeter.Instance.Stability;

        UpdateStabilityText(stability);

        animator.SetInteger("Stability", stability);
    }

    private void StabilityMeter_MinStabilityReady()
    {
        int stability = StabilityMeter.Instance.Stability;

        UpdateStabilityText(stability);

        animator.SetInteger("Stability", stability);
    }
}
