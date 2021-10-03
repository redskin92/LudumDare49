using UnityEngine;

public class ShadowPeopleEffect : StabilityEffect
{
    [SerializeField]
    protected SkinnedMeshRenderer meshRenderer;
    
    [SerializeField]
    protected ParticleSystem particles;

    protected override void Initialze()
    {
        base.Initialze();

        if (StabilityMeter.Instance.Stability > triggerThreshold)
        {
            meshRenderer.enabled = false;
            particles.Stop();
        }
    }

    protected override void EnableEffect()
    {
        base.EnableEffect();

        meshRenderer.enabled = true;
        particles.Play();
    }

    protected override void UpdateEffect()
    {
        base.UpdateEffect();
    }

    protected override void DisableEffect()
    {
        base.DisableEffect();
        
        meshRenderer.enabled = false;
        particles.Stop();
    }
}