using UnityEngine;

public class StabilityEffect : MonoBehaviour
{
    [SerializeField]
    protected float triggerThreshold;

    protected bool active;

    protected void Start()
    {
        Initialze();
    }

    protected virtual void Initialze()
    {
        Register();
        
        UpdateStability();
    }

    protected virtual void UpdateStability()
    {
        if (StabilityMeter.Instance.Stability <= triggerThreshold)
            EnableEffect();
        else
            DisableEffect();
        
        UpdateEffect();
    }

    protected virtual void EnableEffect()
    {
        if (active)
            return;

        active = true;

        Debug.Log(string.Format("Stability Effect {0} has been enabled at {1} stability!"));
    }

    protected virtual void UpdateEffect()
    {
        if (!active)
            return;
        
        Debug.Log(string.Format("Stability Effect {0} has updated its effect at {1} stability!"));
    }

    protected virtual void DisableEffect()
    {
        if (!active)
            return;

        active = false;

        Debug.Log(string.Format("Stability Effect {0} has been disabled at {1} stability!"));
    }

    protected void Register()
    {
        StabilityMeter.Instance.StabilityUpdated += OnStabilityUpdated;
    }

    protected void Unregister()
    {
        StabilityMeter.Instance.StabilityUpdated -= OnStabilityUpdated;
    }

    protected virtual void OnStabilityUpdated()
    {
        UpdateStability();
    }
}