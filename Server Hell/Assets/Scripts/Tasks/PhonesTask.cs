using UnityEngine;

[RequireComponent(typeof(HoldInteractable))]
public class PhonesTask : UrgentTaskBase
{
    public float timeBeforeFail = 10f;
    public GameObject lightObj;

    private ProgressMeter progressMeter;

    private HoldInteractable holdInteractable;
    private bool interactionStarted;

    private AudioSource audioSource;

    public override void ActivateTask()
    {
        Debug.Log("Phones task activated!");
        base.ActivateTask();

        audioSource.Play();
        lightObj.SetActive(true);

        holdInteractable.Interactable = true;

        InvokeRepeating("Fail", timeBeforeFail, timeBeforeFail);
    }

    public void DeactivateTask()
    {
        CancelInvoke("Fail");

        audioSource.Stop();
        lightObj.SetActive(false);

        holdInteractable.Interactable = false;
    }

    private void Awake()
    {
        holdInteractable = GetComponent<HoldInteractable>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        progressMeter = FindObjectOfType<ProgressMeter>();

        holdInteractable.Interactable = false;

        RegisterEvents();
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void Update()
    {
        if (interactionStarted)
            progressMeter.SetProgress(holdInteractable.CurrentProgress);
    }

    private void RegisterEvents()
    {
        holdInteractable.ProgressStarted += HoldInteractable_ProgressStarted;
        holdInteractable.ProgressComplete += HoldInteractable_ProgressComplete;
        holdInteractable.ProgressCanceled += HoldInteractable_ProgressCanceled;
    }

    private void UnregisterEvents()
    {
        holdInteractable.ProgressStarted -= HoldInteractable_ProgressStarted;
        holdInteractable.ProgressComplete -= HoldInteractable_ProgressComplete;
        holdInteractable.ProgressCanceled -= HoldInteractable_ProgressCanceled;
    }

    private void Fail()
    {
        FireTaskFailed();
    }

    private void HoldInteractable_ProgressStarted()
    {
        interactionStarted = true;
    }

    private void HoldInteractable_ProgressComplete()
    {
        interactionStarted = false;

        progressMeter.ProgressComplete();

        DeactivateTask();

        IsActive = false;

        FireTaskComplete();
    }

    private void HoldInteractable_ProgressCanceled()
    {
        interactionStarted = false;

        progressMeter.ResetProgress();
    }
}
