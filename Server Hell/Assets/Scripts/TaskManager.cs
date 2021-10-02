using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public List<TaskBase> routineTasks;
    public List<UrgentTaskBase> urgentTasks;

    [SerializeField]
    private float minUrgentSpawnTime, maxUrgentSpawnTime;

    private List<UrgentTaskBase> activeUrgentTasks = new List<UrgentTaskBase>();

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

        SpawnRandomUrgentAfterDelay();
    }

    private void SpawnRandomUrgentAfterDelay()
    {
        if (urgentTasks.Count == 0)
            return;

        float t = Random.Range(minUrgentSpawnTime, maxUrgentSpawnTime);

        Invoke("ActivateRandomUrgent", t);
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

    private void ActivateRandomUrgent()
    {
        var availableUrgents = urgentTasks.Where(x => !x.IsActive).ToList();

        int count = availableUrgents.Count;

        switch (count)
        {
            case 0:
                Debug.Log("No urgent tasks available!");
                break;
            case 1:
                availableUrgents[0].ActivateTask();
                break;
            default:
                var task = availableUrgents[Random.Range(0, count)];

                task.ActivateTask();
                activeUrgentTasks.Add(task);
                break;
        }
    }

    private void Win()
    {
        UnregisterTaskEvents();

        Debug.Log("You won!");
    }

    private bool CheckWinCondition()
    {
        return completedTasks == tasksToComplete && activeUrgentTasks.Count == 0;
    }

    private void RoutineTask_TaskComplete(TaskBase task)
    {
        task.TaskComplete -= RoutineTask_TaskComplete;

        completedTasks++;
        Debug.Log($"{task.taskName} completed! {completedTasks} out of {tasksToComplete} have been completed.");

        if (CheckWinCondition())
            Win();
    }

    private void UrgentTask_TaskComplete(TaskBase task)
    {
        task.TaskComplete -= UrgentTask_TaskComplete;

        var urgent = task as UrgentTaskBase;
        if (urgent != null)
        {
            if (activeUrgentTasks.Contains(urgent))
                activeUrgentTasks.Remove(urgent);
        }

        if (CheckWinCondition())
            Win();
    }

    private void UrgentTask_TaskFailed(TaskBase task)
    {
        Debug.Log($"{task.taskName} failed!");
    }
}
