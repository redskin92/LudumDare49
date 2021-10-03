using System;
using UnityEngine;

public class StabilityMeter : MonoBehaviour
{
    public static StabilityMeter Instance;

    [SerializeField]
    private int stability = 100;

    [SerializeField]
    private int minStability = 0, maxStability = 100;

    public int Stability
    {
        get { return stability; }
        private set
        {
            if (stability == value)
                return;

            stability = value;

            if (stability < MinStability)
                stability = MinStability;
            else if (stability > MaxStability)
                stability = MaxStability;

            if (stability == MinStability)
            {
                if (MaxStabilityReached != null)
                    MaxStabilityReached();
            }

            if (StabilityUpdated != null)
                StabilityUpdated();

            Debug.Log("Stability at " + stability + "%!");
        }
    }

    public int MinStability
    {
        get { return minStability; }
    }

    public int MaxStability
    {
        get { return maxStability; }
    }

    public event Action StabilityUpdated;
    public event Action MaxStabilityReached;

    public void StabilityIncrease(int increase)
    {
        Stability += increase;
    }
    
    public void StabilityDecrease(int decrease)
    {
        Stability -= decrease;
    }

    private void Awake()
    {
        Instance = this;
    }
}
