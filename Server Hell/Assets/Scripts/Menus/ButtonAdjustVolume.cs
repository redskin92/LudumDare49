using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAdjustVolume : MenuButtons
{
    [SerializeField]
    protected Text volumeText;

    [SerializeField]
    protected float adjustRate = .01f;

    [SerializeField]
    protected bool isMusic = false;

    protected float volume = 1.0f;

    private void Awake()
    {
        if (SoundVolumeController.Instance)
        {
            if (isMusic)
            {
                volume = SoundVolumeController.Instance.musicScale;
            }
            else
            {
                volume = SoundVolumeController.Instance.effectsScale;
            }
        }

        volumeText.text = ((int)(volume * 100)).ToString();
    }

    public void LowerVolume()
    {
        volume -= adjustRate;

        if (volume < 0)
        {
            volume = 0f;
        }
        else
        {
            SetVolume();
        }
    }

    public void RaiseVolume()
    {
        volume += adjustRate;

        if (volume > 1.0f)
        {
            volume = 1.0f;
        }
        else
        {
            SetVolume();
        }
    }

    protected void SetVolume()
    {
        if (SoundVolumeController.Instance)
        {
            if (isMusic)
            {
                SoundVolumeController.Instance.musicScale = volume;
            }
            else
            {
                SoundVolumeController.Instance.effectsScale = volume;

                PlayPressedSound();
            }
        }

        volumeText.text = ((int)(volume * 100)).ToString();
    }
}
