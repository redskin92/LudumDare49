using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example of how to use:
/*  if (SoundVolumeController.Instance)
    {
        SoundVolumeController.Instance.PlayMusic(this);
    }
    else
    {
        source.Play();
    }
*/
/// </summary>


public class SoundVolumeController : MonoBehaviour
{
    public float effectsScale = 1.0f;

    public float musicScale = 1.0f;

    private static SoundVolumeController instance = null;

    // Sound Volume Controller Singleton
    public static SoundVolumeController Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void PlaySound(AdjustedAudioSource sound)
    {
        sound.source.volume = sound.originalVolume * effectsScale;

        sound.source.Play();
    }

    public void PlayMusic(AdjustedAudioSource sound)
    {
        sound.source.volume = sound.originalVolume * musicScale;

        sound.source.Play();
    }
}
