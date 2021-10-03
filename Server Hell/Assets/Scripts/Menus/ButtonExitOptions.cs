using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonExitOptions : MenuButtons
{
    [SerializeField]
    protected OptionMenuController optionsMenuController;


    [SerializeField]
    protected GameObject controlsScemeParent;

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

    public override void Selected(bool playSound = true)
    {
        base.Selected(playSound);

        controlsScemeParent.SetActive(true);
    }

    public override void NotSelected()
    {
        base.NotSelected();

        controlsScemeParent.SetActive(false);
    }
}
