using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HoldInteractable))]
public class PhonesTask : TaskBase
{
    public Image image;

    private HoldInteractable holdInteractable;
    private bool started;

    public override void ActivateTask()
    {
        // TODO
    }

    private void Awake()
    {
        holdInteractable = GetComponent<HoldInteractable>();
    }

    private void Start()
    {
        RegisterEvents();
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void Update()
    {
        if (started)
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

    private void HoldInteractable_ProgressStarted()
    {
        image.gameObject.SetActive(true);

        started = true;
    }

    private void HoldInteractable_ProgressComplete()
    {
        image.gameObject.SetActive(false);

        started = false;

        UnregisterEvents();

        FireTaskComplete();
    }

    private void HoldInteractable_ProgressCanceled()
    {
        image.gameObject.SetActive(false);

        started = false;
    }
}
