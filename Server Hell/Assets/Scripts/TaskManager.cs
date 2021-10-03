using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    public RectTransform routineTasksDisplayParent, urgentTasksDisplayParent;
    public GameObject taskNameDisplayPrefab;

    private List<TaskBase> routineTasks = new List<TaskBase>();
    private List<UrgentTaskBase> urgentTasks = new List<UrgentTaskBase>();

    [SerializeField]
    private float minUrgentSpawnTime, maxUrgentSpawnTime, initialUrgentSpawnTime;

    private List<TaskLabelCount> routineTaskGroup = new List<TaskLabelCount>();
    private List<TaskLabelCount> urgentTaskGroup = new List<TaskLabelCount>();

    private int completedTasks;
    private int tasksToComplete;

    private void Start()
    {
        FindAndAssignTasks();

        tasksToComplete = routineTasks.Count;

        foreach (var task in routineTasks)
        {
            task.TaskComplete += RoutineTask_TaskComplete;
            task.ActivateTask();

            SpawnTaskDisplayUI(routineTasksDisplayParent, task);
        }

        foreach (var task in urgentTasks)
        {
            task.TaskComplete += UrgentTask_TaskComplete;
            task.TaskFailed += UrgentTask_TaskFailed;
        }

        Invoke("ActivateRandomUrgent", initialUrgentSpawnTime);
    }

    private void FindAndAssignTasks()
    {
        routineTasks = new List<TaskBase>();
        urgentTasks = new List<UrgentTaskBase>();

        var tasks = FindObjectsOfType<TaskBase>();

        foreach(var task in tasks)
        {
            var u = task as UrgentTaskBase;
            if (u != null)
                urgentTasks.Add(u);
            else
                routineTasks.Add(task);
        }
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
                ActivateNewUrgentTask(availableUrgents[0]);
                break;
            default:
                var task = availableUrgents[Random.Range(0, count)];

                ActivateNewUrgentTask(task);
                break;
        }

        SpawnRandomUrgentAfterDelay();
    }

    private void Win()
    {
        UnregisterTaskEvents();

        Debug.Log("You won!");
    }

    private bool CheckWinCondition()
    {
        return completedTasks == tasksToComplete && urgentTaskGroup.Count(x => x.task.IsActive) == 0;
    }

    private void ActivateNewUrgentTask(UrgentTaskBase task)
    {
        task.ActivateTask();
        SpawnTaskDisplayUI(urgentTasksDisplayParent, task);
    }

    private void SpawnTaskDisplayUI(RectTransform parent, TaskBase task)
    {
        var group = routineTaskGroup.FirstOrDefault(x => x.task.taskName == task.taskName);
        if (group == null)
        {
            var taskDisplay = Instantiate(taskNameDisplayPrefab, parent).GetComponent<TextMeshProUGUI>();

            group = new TaskLabelCount(task, taskDisplay, 0);
            routineTaskGroup.Add(group);
        }
        else
            group.textMesh.gameObject.SetActive(true);

        group.SetCount(group.count + 1);
    }

    private void SpawnTaskDisplayUI(RectTransform parent, UrgentTaskBase task)
    {
        var group = urgentTaskGroup.FirstOrDefault(x => x.task.taskName == task.taskName);
        if (group == null)
        {
            var taskDisplay = Instantiate(taskNameDisplayPrefab, parent).GetComponent<TextMeshProUGUI>();

            group = new TaskLabelCount(task, taskDisplay, 0);
            urgentTaskGroup.Add(group);
        }
        else
            group.textMesh.gameObject.SetActive(true);

        group.SetCount(group.count + 1);

    }

    private void RoutineTask_TaskComplete(TaskBase task)
    {
        task.TaskComplete -= RoutineTask_TaskComplete;

        var fromList = routineTaskGroup.FirstOrDefault(x => x.task == task);
        if (fromList != null)
        {
            fromList.SetCount(fromList.count - 1);

            if (fromList.count == 0)
                fromList.textMesh.gameObject.SetActive(false);
        }

        completedTasks++;
        Debug.Log($"{task.taskName} completed! {completedTasks} out of {tasksToComplete} have been completed.");

        if (CheckWinCondition())
            Win();
    }

    private void UrgentTask_TaskComplete(TaskBase task)
    {
        var urgent = task as UrgentTaskBase;
        if (urgent != null)
        {
            var fromList = urgentTaskGroup.FirstOrDefault(x => x.task.taskName == task.taskName);
            if (fromList != null)
            {
                fromList.SetCount(fromList.count - 1);

                if (fromList.count == 0)
                    fromList.textMesh.gameObject.SetActive(false);
            }
        }

        if (CheckWinCondition())
            Win();
    }

    private void UrgentTask_TaskFailed(UrgentTaskBase task)
    {
        Debug.Log($"{task.taskName} failed!");
    }

    private class TaskLabelCount
    {
        private const string TASK_FORMAT = "- {0}{1}";

        public TaskBase task;
        public TextMeshProUGUI textMesh;
        public int count;

        public TaskLabelCount(TaskBase t, TextMeshProUGUI tm, int c)
        {
            task = t;
            textMesh = tm;
            SetCount(c);
        }

        public void SetCount(int c)
        {
            count = c;

            if (count < 0)
                count = 0;

            string countSuffix = count <= 1 ? "" : " (" + count + ")";

            textMesh.text = string.Format(TASK_FORMAT, task.taskName, countSuffix);
        }
    }
}
