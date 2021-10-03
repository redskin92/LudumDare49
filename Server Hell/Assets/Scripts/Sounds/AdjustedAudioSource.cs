using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustedAudioSource : MonoBehaviour
{
    public AudioSource source;

    public bool playAtStart = false;

    [HideInInspector]
    public float originalVolume;

    // Awake is called before the first frame update
    void Awake()
    {
        if(!source)
        {
            UnityEngine.Debug.LogWarning("AudioSource attachment missing from object: " + gameObject.name);
            UnityEngine.Debug.LogWarning("Attempting GetComponent on this object.");

            source = GetComponent<AudioSource>();

            if(!source)
            {
                UnityEngine.Debug.LogError("Unable to grab AudioSource from object: " + gameObject.name);
            }
        }

        originalVolume = source.volume;

        if(playAtStart)
        {
            if (SoundVolumeController.Instance)
            {
                SoundVolumeController.Instance.PlayMusic(this);
            }
            else
            {
                source.Play();
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
}