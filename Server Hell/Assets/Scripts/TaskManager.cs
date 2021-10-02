using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public List<TaskBase> tasks;

    private int completedTasks;
    private int tasksToComplete;

    private void Start()
    {
        tasksToComplete = tasks.Count;

        foreach (var task in tasks)
            task.TaskComplete += Task_TaskComplete;
    }

    private void Task_TaskComplete(TaskBase task)
    {
        completedTasks++;
        Debug.Log($"{task.taskName} completed! {completedTasks} out of {tasksToComplete} have been completed.");

        if (completedTasks == tasksToComplete)
            Debug.Log("You won!");
    }
}
