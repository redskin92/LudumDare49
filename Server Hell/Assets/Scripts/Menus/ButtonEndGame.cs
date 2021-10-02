//////////////////////////////////////////////////////////////////////////////////////////
/// Name: ButtonEndGame.cs
/// Description: Handles starting the game with a button.
//////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEndGame : MenuButtons
{
    /// <summary>
    /// End the game!
    /// </summary>
    public override void PressButton()
    {
        PlayPressedSound();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
