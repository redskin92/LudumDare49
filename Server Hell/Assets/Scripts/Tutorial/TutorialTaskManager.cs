using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTaskManager : TaskManager
{
    public TaskBase firstTask;
    public GameObject firstTaskText;

    public TaskBase secondTask;
    public GameObject[] secondTasksEnableObjects, secondTasksDisableObjects;

    public UrgentTaskBase urgentTask;
    public GameObject[] threeTasksEnableObjects, threeTasksDisableObjects;

    public GameObject tutorialCompleteMessage;
    public Image fadeImage;
    public float fadeTime;

    protected override void Start() { }

    public void ActivateFirstTask()
    {
        firstTask.TaskComplete += FirstTask_TaskComplete;
        firstTask.ActivateTask();
        SpawnTaskDisplayUI(routineTasksDisplayParent, firstTask);

        firstTaskText.SetActive(true);
    }

    public void ActivateSecondTask()
    {
        secondTask.TaskComplete += SecondTask_TaskComplete;
        secondTask.ActivateTask();
        SpawnTaskDisplayUI(routineTasksDisplayParent, secondTask);

        foreach (var obj in secondTasksEnableObjects)
            obj.SetActive(true);

        foreach (var obj in secondTasksDisableObjects)
            obj.SetActive(false);
    }

    public void ActivateThirdTask()
    {
        urgentTask.TaskComplete += UrgentTask_TaskComplete;
        urgentTask.ActivateTask();
        SpawnTaskDisplayUI(urgentTasksDisplayParent, urgentTask);

        foreach (var obj in threeTasksEnableObjects)
            obj.SetActive(true);

        foreach (var obj in threeTasksDisableObjects)
            obj.SetActive(false);
    }

    public void FinishTutorial()
    {
        tutorialCompleteMessage.SetActive(true);

        StartCoroutine(FadeOutSequence());
    }

    private IEnumerator FadeOutSequence()
    {
        float alpha = 0;
        Color c = fadeImage.color;
        c.a = alpha;
        while (alpha < 1)
        {
            alpha += Time.deltaTime / fadeTime;

            c.a = alpha;
            fadeImage.color = c;

            yield return null;
        }

        if (LevelManager.Instance != null)
            LevelManager.Instance.TransitionToPlay();
    }

    private void FirstTask_TaskComplete(TaskBase task)
    {
        firstTask.TaskComplete -= FirstTask_TaskComplete;

        firstTaskText.SetActive(false);

        var fromList = routineTaskGroup.FirstOrDefault(x => x.task.taskName == task.taskName);
        if (fromList != null)
        {
            fromList.SetCount(fromList.count - 1);

            if (fromList.count == 0)
                fromList.textMesh.gameObject.SetActive(false);
        }

        ActivateSecondTask();
    }

    private void SecondTask_TaskComplete(TaskBase task)
    {
        secondTask.TaskComplete -= SecondTask_TaskComplete;

        var fromList = routineTaskGroup.FirstOrDefault(x => x.task.taskName == task.taskName);
        if (fromList != null)
        {
            fromList.SetCount(fromList.count - 1);

            if (fromList.count == 0)
                fromList.textMesh.gameObject.SetActive(false);
        }

        ActivateThirdTask();
    }

    private void UrgentTask_TaskComplete(TaskBase task)
    {
        Debug.Log("Woah");
        urgentTask.TaskComplete -= UrgentTask_TaskComplete;

        var fromList = urgentTaskGroup.FirstOrDefault(x => x.task.taskName == task.taskName);
        if (fromList != null)
        {
            fromList.SetCount(fromList.count - 1);

            if (fromList.count == 0)
                fromList.textMesh.gameObject.SetActive(false);
        }

        FinishTutorial();
    }
}
