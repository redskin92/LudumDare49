using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    public RectTransform routineTasksDisplayParent, urgentTasksDisplayParent;
    public GameObject taskNameDisplayPrefab;

    /// <summary>
    /// The point at which we should stop spawning urgent tasks.
    /// </summary>
    public int stabilityThreshold;

    [SerializeField]
    private float minUrgentSpawnTime, maxUrgentSpawnTime, initialUrgentSpawnTime;

    [SerializeField]
    private int numEmailTasks = 3;

    [Tooltip("Dynamically filled.  Don't assign manually.")]
    public List<TaskBase> routineTasks = new List<TaskBase>();

    [Tooltip("Dynamically filled.  Don't assign manually.")]
    public List<UrgentTaskBase> urgentTasks = new List<UrgentTaskBase>();

    public List<TaskLabelCount> routineTaskGroup = new List<TaskLabelCount>();
    private List<TaskLabelCount> urgentTaskGroup = new List<TaskLabelCount>();

    private int completedTasks;
    private int tasksToComplete;

    private string prevTask = string.Empty;

    public event Action TasksCompleted;

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        FindAndAssignTasks();

        tasksToComplete = routineTasks.Count;

        foreach (var task in routineTasks)
        {
            task.TaskComplete += RoutineTask_TaskComplete;
			task.TaskNameUpdate += Routine_TaskNameUpdate;
            task.ActivateTask();

            SpawnTaskDisplayUI(routineTasksDisplayParent, task);
        }

        foreach (var task in urgentTasks)
        {
            task.TaskComplete += UrgentTask_TaskComplete;
            task.TaskFailed += UrgentTask_TaskFailed;
			task.TaskNameUpdate += Urgent_TaskNameUpdate;
        }

        Invoke("ActivateRandomUrgent", initialUrgentSpawnTime);
    }

    private void FindAndAssignTasks()
    {
        routineTasks = new List<TaskBase>();
        urgentTasks = new List<UrgentTaskBase>();

        var tasks = FindObjectsOfType<TaskBase>();

        // Limited e-mail tasks
        var emailTasks = tasks.Where(x => x is EmailsTask).ToList();

        tasks = tasks.Except(emailTasks).ToArray();

        for (int i = 0; i < numEmailTasks; i++)
        {
            int index = UnityEngine.Random.Range(0, emailTasks.Count());

            routineTasks.Add(emailTasks[index]);
            emailTasks.RemoveAt(index);
        }

        emailTasks = null;

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

        float t = UnityEngine.Random.Range(minUrgentSpawnTime, maxUrgentSpawnTime);

        Invoke("ActivateRandomUrgent", t);
    }

    private void UnregisterTaskEvents()
    {
		foreach (var task in routineTasks)
		{
			task.TaskComplete -= RoutineTask_TaskComplete;
			task.TaskNameUpdate -= Routine_TaskNameUpdate;
		}

        foreach (var task in urgentTasks)
        {
            task.TaskComplete -= UrgentTask_TaskComplete;
            task.TaskFailed -= UrgentTask_TaskFailed;
			task.TaskNameUpdate -= Urgent_TaskNameUpdate;
        }
    }

    private void ActivateRandomUrgent()
    {
        if (StabilityMeter.Instance != null && StabilityMeter.Instance.Stability > stabilityThreshold)
        {
            List<UrgentTaskBase> availableUrgents;
            int count = urgentTasks.Count(x => !x.IsActive && x.taskName != prevTask);
            if (count > 0)
                availableUrgents = urgentTasks.Where(x => !x.IsActive && x.taskName != prevTask).ToList();
            else
                availableUrgents = urgentTasks.Where(x => !x.IsActive).ToList();

            switch (count)
            {
                case 0:
                    Debug.Log("No urgent tasks available!");
                    break;
                case 1:
                    ActivateNewUrgentTask(availableUrgents[0]);
                    break;
                default:
                    var taskNames = availableUrgents.Select(x => x.taskName).Distinct();
                    string taskName = taskNames.ElementAt(UnityEngine.Random.Range(0, taskNames.Count()));

                    var tasksByName = availableUrgents.Where(x => x.taskName == taskName);

                    var task = availableUrgents.ElementAt(UnityEngine.Random.Range(0, tasksByName.Count()));

                    ActivateNewUrgentTask(task);
                    break;
            }
        }

        SpawnRandomUrgentAfterDelay();
    }

    private void Win()
    {
        UnregisterTaskEvents();
        CancelInvoke("ActivateRandomUrgent");

        Debug.Log("All tasks completed!");

        if (TasksCompleted != null)
            TasksCompleted();
    }

    private bool CheckWinCondition()
    {
        return completedTasks == tasksToComplete && urgentTaskGroup.Count(x => x.task.IsActive) == 0;
    }

    private void ActivateNewUrgentTask(UrgentTaskBase task)
    {
        task.ActivateTask();
        SpawnTaskDisplayUI(urgentTasksDisplayParent, task);

        prevTask = task.taskName;
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

        var fromList = routineTaskGroup.FirstOrDefault(x => x.task.taskName == task.taskName);
        if (fromList != null)
        {
            fromList.SetCount(fromList.count - 1);

            if (fromList.count == 0)
                fromList.textMesh.gameObject.SetActive(false);
        }

        completedTasks++;
        Debug.Log($"{task.taskName} completed! {completedTasks} out of {tasksToComplete} have been completed.");

        if (StabilityMeter.Instance != null)
            StabilityMeter.Instance.StabilityIncrease(task.stabilityGainOnSuccess);

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

        Debug.Log($"{task.taskName} completed!");

        if (StabilityMeter.Instance != null)
            StabilityMeter.Instance.StabilityIncrease(task.stabilityGainOnSuccess);

        if (CheckWinCondition())
            Win();
    }

    private void UrgentTask_TaskFailed(UrgentTaskBase task)
    {
        Debug.Log($"{task.taskName} failed!");

        if (StabilityMeter.Instance != null)
            StabilityMeter.Instance.StabilityDecrease(task.stabilityLossOnFail);
    }

	private void Routine_TaskNameUpdate(TaskBase task)
	{
		var group = routineTaskGroup.FirstOrDefault(x => x.task.taskName == task.taskName);
		if (group == null) return;

		group.UpdateText();
	}

	private void Urgent_TaskNameUpdate(TaskBase task)
	{
		var group = urgentTaskGroup.FirstOrDefault(x => x.task.taskName == task.taskName);
		if (group == null) return;

		group.UpdateText();
	}

	public class TaskLabelCount
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

			UpdateText();
		}

		public void UpdateText()
		{
			string countSuffix = count <= 1 ? "" : " (" + count + ")";

			textMesh.text = string.Format(TASK_FORMAT, task.taskName, countSuffix);
		}
    }
}
