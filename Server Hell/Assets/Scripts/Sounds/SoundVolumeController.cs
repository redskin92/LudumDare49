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

    protected AdjustedAudioSource currentMusic = null;

    protected float lastMusicVolume;

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

        lastMusicVolume = musicScale;
    }

    private void FixedUpdate()
    {
        if(lastMusicVolume != musicScale)
        {
            lastMusicVolume = musicScale;

            AdjustMusic();
        }
    }

    public void PlaySound(AdjustedAudioSource sound)
    {
        sound.source.volume = sound.originalVolume * effectsScale;

        sound.source.Play();
    }

    public void AdjustMusic()
    {
        if (currentMusic)
        {
            currentMusic.source.volume = currentMusic.originalVolume * musicScale;
        }
    }

    public void PlayMusic(AdjustedAudioSource sound)
    {
        if(currentMusic)
        {
            currentMusic.Stop();
        }

        sound.source.volume = sound.originalVolume * musicScale;

        sound.source.Play();

        currentMusic = sound;
    }
}
