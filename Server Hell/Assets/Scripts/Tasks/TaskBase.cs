using System;
using UnityEngine;

public abstract class TaskBase : MonoBehaviour
{
    public string taskName;

    public int stabilityGainOnSuccess;

    [NonSerialized]
    public bool completed = false;

    public bool IsActive { get; protected set; }

    public event Action<TaskBase> TaskComplete;

    public void FireTaskComplete()
    {
        completed = true;

        if (TaskComplete != null)
            TaskComplete(this);
    }

    /// <summary>
    /// Make this task become active.
    /// </summary>
    public virtual void ActivateTask()
    {
        IsActive = true;
    }
}
