using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonReplayGame : MenuButtons
{
    /// <summary>
    /// Exit options!
    /// </summary>
    public override void PressButton()
    {
        PlayPressedSound();

        if(LevelManager.Instance)
            LevelManager.Instance.TransitionToPlay();
    }
}
