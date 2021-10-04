using UnityEngine;

public class ShadowPeopleEffect : StabilityEffect
{
    [SerializeField]
    protected SkinnedMeshRenderer meshRenderer;

    [SerializeField]
    protected ParticleSystem particles;

    protected override void DoInitialize()
    {
        if (StabilityMeter.Instance.Stability > triggerThreshold)
        {
            meshRenderer.enabled = false;
            particles.Stop();
        }
    }

    protected override void DoEnableEffect()
    {
        meshRenderer.enabled = true;
        particles.Play();
    }

    protected override void DoUpdateEffect() { }

    protected override void DoDisableEffect()
    {
        meshRenderer.enabled = false;
        particles.Stop();
    }
}