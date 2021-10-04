using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HoldInteractable))]
public class LeakTask : UrgentTaskBase
{
    public float timeBeforeFail = 10f;

    [SerializeField]
    private GameObject minimapIndicator;
    
    private ProgressMeter progressMeter;
    private HoldInteractable holdInteractable;
    private bool interactionStarted;

    private ParticleSystem waterSprayParticles;

    public override void ActivateTask()
    {
        Debug.Log("Leak task activated!");
        base.ActivateTask();

        holdInteractable.Interactable = true;
        waterSprayParticles.Play();
        
        minimapIndicator.SetActive(true);

        InvokeRepeating("Fail", timeBeforeFail, timeBeforeFail);
    }

    public void DeactivateTask()
    {
        CancelInvoke("Fail");
        holdInteractable.Interactable = false;
        waterSprayParticles.Stop();
        minimapIndicator.SetActive(false);
    }

    private void Awake()
    {
        holdInteractable = GetComponent<HoldInteractable>();
        waterSprayParticles = GetComponentInChildren<ParticleSystem>();
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
