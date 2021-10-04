//////////////////////////////////////////////////////////////////////////////////////////
/// Name: ButtonStartGame.cs
/// Description: Handles starting the game with a button.
//////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStartGame : MenuButtons
{
    [SerializeField]
    protected AdjustedAudioSource mainMenuMusic;

    /// <summary>
    /// Start the game!
    /// </summary>
    public override void PressButton()
    {
        mainMenuMusic.FadeSound();

        PlayPressedSound();
        LevelManager.Instance.TransitionToPlay();
    }
}
