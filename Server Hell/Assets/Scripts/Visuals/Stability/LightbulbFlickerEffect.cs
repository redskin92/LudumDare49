using System.Collections;
using UnityEngine;

public class LightbulbFlickerEffect : StabilityEffect
{
    [SerializeField]
    private CeilingLight ceilingLight;

    [SerializeField]
    private AnimationCurve flickerCurve;

    [SerializeField]
    private float minTimeBetweenFlickers = 5.0f;

    [SerializeField]
    private float maxTimeBetweenFlickers = 20.0f;

    [SerializeField]
    private float sequenceDuration = 1.0f;

    private float cooldown;

    private bool playingSequence;

    protected void Update()
    {
        cooldown -= Time.deltaTime;
    }

    protected override void DoInitialize() { }

    protected override void DoEnableEffect()
    {
        PlayEffectSequence();
    }

    protected override void DoUpdateEffect()
    {
        PlayEffectSequence();
    }

    protected override void DoDisableEffect()
    {
        if (!playingSequence)
            return;

        playingSequence = false;
        StopAllCoroutines();

        cooldown = 0.0f;
    }

    private void PlayEffectSequence()
    {
        if (!playingSequence && cooldown <= 0.0f)
        {
            playingSequence = true;
            StartCoroutine(EffectSequence());
        }
    }

    private IEnumerator EffectSequence()
    {
        float time = 0.0f;
        while (time < sequenceDuration)
        {
            ceilingLight.SetLightIntensity(flickerCurve.Evaluate(time / sequenceDuration));
            time += Time.deltaTime;
            yield return null;
        }

        playingSequence = false;
        cooldown = Random.Range(minTimeBetweenFlickers, maxTimeBetweenFlickers);
    }
}