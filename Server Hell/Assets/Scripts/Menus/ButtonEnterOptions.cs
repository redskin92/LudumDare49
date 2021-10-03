using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEnterOptions : MenuButtons
{
    [SerializeField]
    protected MenuButtonController baseMenuButtonController;

    /// <summary>
    /// Enter options!
    /// </summary>
    public override void PressButton()
    {
        if (SoundVolumeController.Instance)
        {
            SoundVolumeController.Instance.EnableOptions(baseMenuButtonController);
            PlayPressedSound();
        }
        else
        {
            UnityEngine.Debug.Log("Add a SoundVolumeController object to any scene to be able to load in Options!");
        }
    }
}
