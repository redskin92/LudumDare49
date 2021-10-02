using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HoldInteractable))]
public class PhonesTask : UrgentTaskBase
{
    public Image image;
    public float timeBeforeFail = 10f;

    private HoldInteractable holdInteractable;
    private bool interactionStarted;

    private void Awake()
    {
        holdInteractable = GetComponent<HoldInteractable>();
    }

    private void Start()
    {
        RegisterEvents();

        InvokeRepeating("Fail", timeBeforeFail, timeBeforeFail);
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

        UnregisterEvents();

        IsActive = false;

        FireTaskComplete();
    }

    private void HoldInteractable_ProgressCanceled()
    {
        image.gameObject.SetActive(false);

        interactionStarted = false;
    }
}
