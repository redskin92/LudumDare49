using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMainMenu : MenuButtons
{
    [SerializeField]
    protected AdjustedAudioSource loseGameMusic;

    [SerializeField]
    protected AdjustedAudioSource winGameMusic;

    /// <summary>
    /// Enter options!
    /// </summary>
    public override void PressButton()
    {
        if (LevelManager.Instance)
            LevelManager.Instance.TransitionToMain();

        if (SoundVolumeController.Instance)
        {
            PlayPressedSound();

            if (loseGameMusic)
            {
                if (loseGameMusic.source.isPlaying)
                {
                    loseGameMusic.FadeSound();
                }
            }

            if (winGameMusic)
            {
                if (winGameMusic.source.isPlaying)
                {
                    winGameMusic.FadeSound();
                }
            }
        }
        else
        {
            UnityEngine.Debug.Log("Add a SoundVolumeController object to any scene to be able to load in Options!");
        }
    }
}
