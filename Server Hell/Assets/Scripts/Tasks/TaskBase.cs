using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskBase : MonoBehaviour
{
    public string taskName;

    [NonSerialized]
    public bool completed = false;

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
    public abstract void ActivateTask();
}
