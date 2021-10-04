using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonReplayGame : MenuButtons
{
    [SerializeField]
    protected AdjustedAudioSource loseGameMusic;

    [SerializeField]
    protected AdjustedAudioSource winGameMusic;

    /// <summary>
    /// Exit options!
    /// </summary>
    public override void PressButton()
    {
        PlayPressedSound();

        if (LevelManager.Instance)
        {
            LevelManager.Instance.TransitionToPlay();

            if (SoundVolumeController.Instance)
            {
                if (loseGameMusic.source.isPlaying)
                {
                    loseGameMusic.FadeSound();
                }

                if (winGameMusic.source.isPlaying)
                {
                    winGameMusic.FadeSound();
                }
            }
            else
            {
                UnityEngine.Debug.Log("Add a SoundVolumeController object to any scene to be able to load in Options!");
            }
        }
    }
}
