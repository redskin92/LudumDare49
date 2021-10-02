using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UrgentTaskBase : TaskBase
{
    public event Action<TaskBase> TaskFailed;
}
