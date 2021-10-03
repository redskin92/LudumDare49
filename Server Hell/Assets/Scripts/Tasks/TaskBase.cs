using System;
using UnityEngine;

public abstract class TaskBase : MonoBehaviour
{
    public string taskName;

    public int stabilityGainOnSuccess;

	public bool IsActive { get; protected set; }

    public event Action<TaskBase> TaskComplete;
	public event Action<TaskBase> TaskNameUpdate;

    public void FireTaskComplete()
    {
		IsActive = false;

        if (TaskComplete != null)
            TaskComplete(this);
    }

	public void UpdateTaskName(string newName)
	{
		taskName = newName;

		if (TaskNameUpdate != null)
			TaskNameUpdate(this);
	}

    /// <summary>
    /// Make this task become active.
    /// </summary>
    public virtual void ActivateTask()
    {
        IsActive = true;
    }
}
