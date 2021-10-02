//////////////////////////////////////////////////////////////////////////////////////////
/// Name: ButtonStartGame.cs
/// Description: Handles starting the game with a button.
//////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStartGame : MenuButtons
{
    /// <summary>
    /// Start the game!
    /// </summary>
    public override void PressButton()
    {
        PlayPressedSound();
        //GameFlow.Instance.StartGame();
        gameObject.SetActive(false);
    }
}
