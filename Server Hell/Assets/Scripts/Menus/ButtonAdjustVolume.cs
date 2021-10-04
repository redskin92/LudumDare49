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

    [SerializeField]
    protected GameObject controlsScemeParent;

    protected float volume = 1.0f;

    protected float volumeAlphaStart;

    protected virtual void Start()
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

        if (volumeText)
        {
            volumeAlphaStart = volumeText.color.a;

            volumeText.text = ((int)(volume * 100)).ToString();
        }
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

    public override void Selected(bool playSound = true)
    {
        base.Selected(playSound);

        controlsScemeParent.SetActive(true);

        if (volumeText)
        {
            Color color = selectedColor;

            color.a = 1.0f;

            volumeText.color = color;
        }
    }

    public override void NotSelected()
    {
        base.NotSelected();

        controlsScemeParent.SetActive(false);

        if (volumeText)
        {
            Color color = notSelectedColor;

            color.a = volumeAlphaStart;

            volumeText.color = color;
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

                if(!buttonSelectedSound.source.isPlaying)
                    PlayPressedSound();
            }
        }

        volumeText.text = ((int)(volume * 100)).ToString();
    }
}
