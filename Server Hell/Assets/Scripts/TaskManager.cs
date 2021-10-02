using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public List<TaskBase> routineTasks;
    public List<UrgentTaskBase> urgentTasks;

    private int completedTasks;
    private int tasksToComplete;

    private void Start()
    {
        tasksToComplete = routineTasks.Count;

        foreach (var task in routineTasks)
        {
            task.TaskComplete += RoutineTask_TaskComplete;
            task.ActivateTask();
        }

        foreach (var task in urgentTasks)
        {
            task.TaskComplete += UrgentTask_TaskComplete;
            task.TaskFailed += UrgentTask_TaskFailed;
        }
    }

    private void UnregisterTaskEvents()
    {
        foreach (var task in routineTasks)
            task.TaskComplete -= RoutineTask_TaskComplete;

        foreach (var task in urgentTasks)
        {
            task.TaskComplete -= UrgentTask_TaskComplete;
            task.TaskFailed -= UrgentTask_TaskFailed;
        }
    }

    private void RoutineTask_TaskComplete(TaskBase task)
    {
        task.TaskComplete -= RoutineTask_TaskComplete;

        completedTasks++;
        Debug.Log($"{task.taskName} completed! {completedTasks} out of {tasksToComplete} have been completed.");

        if (completedTasks == tasksToComplete)
            Debug.Log("You won!");
    }

    private void UrgentTask_TaskComplete(TaskBase task)
    {
        task.TaskComplete -= UrgentTask_TaskComplete;

        Debug.Log($"{task.taskName} completed! {completedTasks} out of {tasksToComplete} have been completed.");

        if (completedTasks == tasksToComplete)
            Debug.Log("You won!");
    }

    private void UrgentTask_TaskFailed(TaskBase task)
    {
        UnregisterTaskEvents();

        Debug.Log($"{task.taskName} failed!  You lose!");
    }
}
