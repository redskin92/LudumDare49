using System;

public abstract class UrgentTaskBase : TaskBase
{
    public int stabilityLossOnFail;

    public event Action<UrgentTaskBase> TaskFailed;

	public void FireTaskFailed()
	{
		completed = true;

		if (TaskFailed != null)
			TaskFailed(this);
	}
}
