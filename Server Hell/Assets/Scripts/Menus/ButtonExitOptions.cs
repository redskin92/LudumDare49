using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonExitOptions : MenuButtons
{
    [SerializeField]
    protected OptionMenuController optionsMenuController;

    /// <summary>
    /// Exit options!
    /// </summary>
    public override void PressButton()
    {
        PlayPressedSound();

        if(optionsMenuController)
        {
            optionsMenuController.DisableOptionsMenu();
        }
    }
}
