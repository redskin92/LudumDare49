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

    [SerializeField]
    protected Image buttonImage;

    protected float startingImageAlpha;

    protected float startingTextAlpha;

    protected Color selectedColor = Color.green;

    protected Color notSelectedColor = Color.red;

    protected virtual void Awake()
    {
        if (buttonImage)
        {
            startingImageAlpha = buttonImage.color.a;

            startingTextAlpha = buttonText.color.a;
        }
    }

    /// <summary>
    /// Do the action.
    /// </summary>
    public virtual void PressButton()
    {
    }

    /// <summary>
    /// Turn the text green to signify selected.
    /// </summary>
    public virtual void Selected(bool playSound = true)
    {

        if(buttonImage)
        {
            Color color = buttonImage.color;

            color.a = 1.0f;

            buttonImage.color = color;
        }

        if(buttonText)
        {
            Color color = selectedColor;

            color.a = 1.0f;

            buttonText.color = color;
        }
    }

    /// <summary>
    /// Turn the text red to signify not selected.
    /// </summary>
    public virtual void NotSelected()
    {
        buttonText.color = Color.red;

        if (buttonImage)
        {
            Color color = buttonImage.color;

            color.a = startingImageAlpha;

            buttonImage.color = color;
        }

        if (buttonText)
        {
            Color color = notSelectedColor;

            color.a = startingTextAlpha;

            buttonText.color = color;
        }
    }

    public void PlayPressedSound()
    {
        if (buttonSelectedSound)
        {
            if (SoundVolumeController.Instance)
            {
                SoundVolumeController.Instance.PlaySound(buttonSelectedSound);
            }
            else
            {
                buttonSelectedSound.Play();
            }
        }
    }

    public void DisableButton()
    {

    }
}
