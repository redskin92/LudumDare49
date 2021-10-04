using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChromaticAberrationEffect : StabilityEffect
{
    [SerializeField]
    protected Volume volume;

    protected ChromaticAberration chromaticAberration;

    protected override void DoInitialize()
    {
        // Destroy if attached volume doesn't have chromatic aberration.
        if (!volume.sharedProfile.TryGet(out chromaticAberration)) Destroy(this);

        if (chromaticAberration != null)
        {
            chromaticAberration.active = true;
            chromaticAberration.intensity.value = 0.0f;
        }
    }

    protected override void DoEnableEffect()
    {
        if (chromaticAberration == null)
            active = false;
    }

    protected override void DoUpdateEffect()
    {
        chromaticAberration.intensity.value = 1.0f - StabilityMeter.Instance.Stability / (float) StabilityMeter.Instance.MaxStability;
    }

    protected override void DoDisableEffect()
    {
        chromaticAberration.intensity.value = 0.0f;
    }
}