using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGameEnd : MonoBehaviour
{
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        StabilityMeter.Instance.MinStabilityReached += StabilityMeter_MinStabilityReached;
        TaskManager.Instance.TasksCompleted += TaskManager_TasksCompleted;
    }

    private void OnDestroy()
    {
        if (StabilityMeter.Instance != null)
            StabilityMeter.Instance.MinStabilityReached -= StabilityMeter_MinStabilityReached;

        if (TaskManager.Instance != null)
            TaskManager.Instance.TasksCompleted -= TaskManager_TasksCompleted;
    }

    private void StabilityMeter_MinStabilityReached()
    {
        playerInput.actions.Disable();
    }

    private void TaskManager_TasksCompleted()
    {
        playerInput.actions.Disable();
    }
}
