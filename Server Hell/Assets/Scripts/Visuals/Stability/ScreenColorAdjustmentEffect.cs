using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenColorAdjustmentEffect : StabilityEffect
{
    [SerializeField]
    protected Volume volume;

    [SerializeField]
    [Range(0, 100)]
    protected float effectPercent = 100f;

    protected ColorAdjustments colorAdjustments;

    protected override void DoInitialize()
    {
        // Destroy if attached volume doesn't have film grain effect.
        if (!volume.sharedProfile.TryGet(out colorAdjustments)) Destroy(this);

        if (colorAdjustments != null)
        {
            colorAdjustments.active = true;
            colorAdjustments.saturation.value = 0.0f;
        }
    }

    protected override void DoEnableEffect()
    {
        if (colorAdjustments == null)
            active = false;
    }

    protected override void DoUpdateEffect()
    {
        colorAdjustments.saturation.value = (-1.0f + StabilityMeter.Instance.Stability / (float) StabilityMeter.Instance.MaxStability) * effectPercent;
    }

    protected override void DoDisableEffect()
    {
        colorAdjustments.saturation.value = 0.0f;
    }
}