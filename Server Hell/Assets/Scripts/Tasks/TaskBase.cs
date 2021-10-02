using System;
using UnityEngine;

public abstract class TaskBase : MonoBehaviour
{
    public string taskName;

    public bool IsActive { get; protected set; }

    public event Action<TaskBase> TaskComplete;

    public void FireTaskComplete()
    {
		IsActive = false;

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
