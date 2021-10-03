using UnityEngine;
using UnityEngine.UI;

public class ProgressMeter : MonoBehaviour
{
    private Image image;

    public void SetProgress(float progress)
    {
        image.fillAmount = progress;
    }

    public void ProgressComplete()
    {
        image.fillAmount = 0;
    }

    public void ResetProgress()
    {
        image.fillAmount = 0;
    }

    private void Awake()
    {
        image = GetComponent<Image>();
    }
}
