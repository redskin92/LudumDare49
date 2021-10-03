using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HoldInteractable))]
public class LeakTask : UrgentTaskBase
{
    public Image image;
    public float timeBeforeFail = 10f;

    private HoldInteractable holdInteractable;
    private bool interactionStarted;

    private ParticleSystem waterSprayParticles;

    public override void ActivateTask()
    {
        Debug.Log("Leak task activated!");
        base.ActivateTask();

        holdInteractable.Interactable = true;
        waterSprayParticles.Play();

        InvokeRepeating("Fail", timeBeforeFail, timeBeforeFail);
    }

    public void DeactivateTask()
    {
        CancelInvoke("Fail");
        holdInteractable.Interactable = false;
        waterSprayParticles.Stop();
    }

    private void Awake()
    {
        holdInteractable = GetComponent<HoldInteractable>();
        waterSprayParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
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
            image.fillAmount = holdInteractable.CurrentProgress;
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
        image.gameObject.SetActive(true);

        interactionStarted = true;
    }

    private void HoldInteractable_ProgressComplete()
    {
        image.gameObject.SetActive(false);

        interactionStarted = false;

        DeactivateTask();

        IsActive = false;

        FireTaskComplete();
    }

    private void HoldInteractable_ProgressCanceled()
    {
        image.gameObject.SetActive(false);

        interactionStarted = false;
    }
}
