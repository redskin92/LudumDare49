using UnityEngine;

public abstract class StabilityEffect : MonoBehaviour
{
    [SerializeField]
    protected float triggerThreshold;

    protected bool initialized;
    protected bool active;

    protected void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (initialized)
            return;

        initialized = true;
        
        Register();
        
        UpdateStability();
        
        DoInitialize();
    }

    protected abstract void DoInitialize();

    private void UpdateStability()
    {
        if (!initialized)
            return;
        
        if (StabilityMeter.Instance.Stability <= triggerThreshold)
            EnableEffect();
        else
            DisableEffect();
        
        UpdateEffect();
    }

    protected void EnableEffect()
    {
        if (!initialized)
            return;
        
        if (active)
            return;

        active = true;
        
        DoEnableEffect();
    }

    protected abstract void DoEnableEffect();

    protected void UpdateEffect()
    {
        if (!initialized)
            return;
        
        if (!active)
            return;
        
        DoUpdateEffect();
    }

    protected abstract void DoUpdateEffect();

    protected void DisableEffect()
    {
        if (!initialized)
            return;
        
        if (!active)
            return;

        active = false;
        
        DoDisableEffect();
    }
    
    protected abstract void DoDisableEffect();

    protected void Register()
    {
        StabilityMeter.Instance.StabilityUpdated += OnStabilityUpdated;
    }

    protected void Unregister()
    {
        StabilityMeter.Instance.StabilityUpdated -= OnStabilityUpdated;
    }

    protected void OnStabilityUpdated()
    {
        if (!initialized)
            return;
        
        UpdateStability();
    }
}