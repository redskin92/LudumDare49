using System.Collections;
using UnityEngine;

public class AdjustedAudioSource : MonoBehaviour
{
    public AudioSource source;

    public bool isMusic = false;

    public float fadeSoundTime = 0.5f;

    [HideInInspector]
    public float originalVolume;

    // Awake is called before the first frame update
    void Awake()
    {
        if(!source)
        {
            source = GetComponent<AudioSource>();

            if(!source)
            {
                UnityEngine.Debug.LogError("Unable to grab AudioSource from object: " + gameObject.name);
            }
        }

        originalVolume = source.volume;

        if(isMusic)
        {
            if (SoundVolumeController.Instance)
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
        StartCoroutine(FadeSoundOverTime());
    }

    private IEnumerator FadeSoundOverTime()
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
}
