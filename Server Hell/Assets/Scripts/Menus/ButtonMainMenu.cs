using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMainMenu : MenuButtons
{
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
        }
        else
        {
            UnityEngine.Debug.Log("Add a SoundVolumeController object to any scene to be able to load in Options!");
        }
    }
}
