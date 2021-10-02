using System;

public abstract class UrgentTaskBase : TaskBase
{
    public int stabilityLossOnFail;

    public event Action<UrgentTaskBase> TaskFailed;

	public void FireTaskFailed()
	{
		if (TaskFailed != null)
			TaskFailed(this);
	}
}
