using System.Collections;
using UnityEngine;

public class AdjustedAudioSource : MonoBehaviour
{
    public AudioSource source;

    public bool isMusic = false;

    public bool playOnStart = false;

    public float fadeSoundTime = 0.5f;

    [HideInInspector]
    public float originalVolume;

    // Awake is called before the first frame update
    void Awake()
    {
        if (!source)
        {
            source = GetComponent<AudioSource>();

            if (!source)
            {
                UnityEngine.Debug.LogError("Unable to grab AudioSource from object: " + gameObject.name);
            }
        }

        originalVolume = source.volume;

        if (SoundVolumeController.Instance)
        {
            if (playOnStart)
            {
                SoundVolumeController.Instance.PlayMusic(this);

            }
            else if (isMusic)
            {
                SoundVolumeController.Instance.SetMusic(this);
            }
        }
    }

    public void Play()
    {
        if(source)
        {
            source.Play();
        }
    }

    public void Stop()
    {
        if (source)
        {
            source.Stop();
        }

        StopAllCoroutines();
    }

    public void FadeSound()
    {
        StartCoroutine(FadeSoundOutOverTime());
    }

    private IEnumerator FadeSoundOutOverTime()
    {
        float time = 0;

        float startingVolume = source.volume;

        while (time < fadeSoundTime)
        {
            time += Time.deltaTime;
            source.volume = startingVolume * ((fadeSoundTime - time) / fadeSoundTime);
            yield return null;
        }

        Stop();

        yield return null;
    }

    public void FadeSoundIn()
    {
        StartCoroutine(FadeSoundInOverTime());
    }

    private IEnumerator FadeSoundInOverTime()
    {
        float time = 0;

        float endingVolume = source.volume;

        Play();

        while (time < fadeSoundTime)
        {
            time += Time.deltaTime;
            source.volume = endingVolume * (time / fadeSoundTime);
            yield return null;
        }

        yield return null;
    }
}
