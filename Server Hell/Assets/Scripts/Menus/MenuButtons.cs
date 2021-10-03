//////////////////////////////////////////////////////////////////////////////////////////
/// Name: MenuButtons.cs
/// Description: Handles each button individually.
//////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuButtons : MonoBehaviour
{
    [SerializeField]
    protected Text buttonText;

    [SerializeField]
    protected AdjustedAudioSource buttonSelectedSound;

    /// <summary>
    /// Do the action.
    /// </summary>
    public virtual void PressButton()
    {
        UnityEngine.Debug.Log("No action selected for this button: " + name);
    }

    /// <summary>
    /// Turn the text green to signify selected.
    /// </summary>
    public void Selected(bool playSound = true)
    {
        buttonText.color = Color.green;
    }

    /// <summary>
    /// Turn the text red to signify not selected.
    /// </summary>
    public void NotSelected()
    {
        buttonText.color = Color.red;
    }

    public void PlayPressedSound()
    {
        if (buttonSelectedSound)
        {
            if (SoundVolumeController.Instance)
            {
                SoundVolumeController.Instance.PlayMusic(buttonSelectedSound);
            }
            else
            {
                buttonSelectedSound.Play();
            }
        }
    }
}
