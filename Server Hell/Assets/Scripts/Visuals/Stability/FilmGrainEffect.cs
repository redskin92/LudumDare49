using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FilmGrainEffect : StabilityEffect
{
    [SerializeField]
    protected Volume volume;

    protected FilmGrain filmGrain;

    protected override void DoInitialize()
    {
        // Destroy if attached volume doesn't have film grain effect.
        if (!volume.sharedProfile.TryGet(out filmGrain)) Destroy(this);

        if (filmGrain != null)
        {
            filmGrain.active = true;
            filmGrain.intensity.value = 0.0f;
        }
    }

    protected override void DoEnableEffect()
    {
        if (filmGrain == null)
            active = false;
    }

    protected override void DoUpdateEffect()
    {
        filmGrain.intensity.value = 1.0f - StabilityMeter.Instance.Stability / (float) StabilityMeter.Instance.MaxStability;
    }

    protected override void DoDisableEffect()
    {
        filmGrain.intensity.value = 0.0f;
    }
}