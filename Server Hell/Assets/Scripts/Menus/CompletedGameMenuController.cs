using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CompletedGameMenuController : MenuButtonController
{
    [SerializeField]
    protected GameObject completedGameMenuBase;

    [SerializeField]
    protected Text titleText;

    [SerializeField]
    protected string winText;

    [SerializeField]
    protected string loseText;

    [SerializeField]
    protected TaskManager taskManager;

    [SerializeField]
    protected StabilityMeter stabilityMeter;

    [SerializeField]
    protected float seperationAtOne = 200.0f;

    [SerializeField]
    protected GameObject textPrefab;

    [SerializeField]
    protected GameObject wonTitleObject;

    public Color completeColor = Color.green;

    public Color notCompleteColor = Color.red;

    public InputAction testIncreaseStability;

    public InputAction testDecreaseStability;

    public int stabilityIncrement = 10;




    protected override void Awake()
    {
        base.Awake();

        if(!taskManager)
        {
            taskManager = FindObjectsOfType(typeof(TaskManager))[0] as TaskManager;
        }

        if (!stabilityMeter)
        {
            stabilityMeter = FindObjectsOfType(typeof(StabilityMeter))[0] as StabilityMeter;
        }

        completedGameMenuBase.SetActive(false);

        taskManager.TasksCompleted += WonGame;

        stabilityMeter.MinStabilityReached += LostGame;
    }

    protected override void OnEnable()
    {
        EnableButtons();

        testIncreaseStability.Enable();
        testDecreaseStability.Enable();

        testIncreaseStability.canceled += IncreaseStability;
        testDecreaseStability.canceled += DecreaseStability;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        testIncreaseStability.canceled -= IncreaseStability;
        testDecreaseStability.canceled -= DecreaseStability;

        testIncreaseStability.Disable();
        testDecreaseStability.Disable();
    }

    public void WonGame()
    {
        taskManager.TasksCompleted -= WonGame;

        if (titleText)
        {
            titleText.text = winText;
        }

        ListObjectives();

        completedGameMenuBase.SetActive(true);

        RegisterButtons();
    }

    public void LostGame()
    {
        stabilityMeter.MinStabilityReached -= LostGame;

        if (titleText)
        {
            titleText.text = loseText;
        }

        ListObjectives();

        completedGameMenuBase.SetActive(true);

        RegisterButtons();
    }

    public void IncreaseStability(InputAction.CallbackContext obj)
    {
        if(stabilityMeter)
        {
            stabilityMeter.StabilityIncrease(stabilityIncrement);
        }
    }

    public void DecreaseStability(InputAction.CallbackContext obj)
    {
        if(stabilityMeter)
        {
            stabilityMeter.StabilityDecrease(stabilityIncrement);
        }
    }

    protected void ListObjectives()
    {
        List<TaskManager.TaskLabelCount> routineTasks = taskManager.routineTaskGroup;

        int routineTasksCount = routineTasks.Count;

        float seperation = seperationAtOne / routineTasksCount;

        float currentSeperation = seperation;

        for (int i = 0; i < routineTasks.Count; ++i)
        {
            GameObject taskTitle = Instantiate(textPrefab, wonTitleObject.transform);

            Text taskTitlteText = taskTitle.GetComponent<Text>();

            if (taskTitlteText)
            {
                taskTitlteText.text = routineTasks[i].textMesh.text;

                Color color = notCompleteColor;

                if(routineTasks[i].count <= 0)
                {
                    color = completeColor;
                }

                taskTitlteText.color = color;
            }

            taskTitle.transform.localPosition = new Vector3(0, -currentSeperation, 0);

            currentSeperation += seperation;
        }
    }
}
