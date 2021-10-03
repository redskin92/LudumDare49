//////////////////////////////////////////////////////////////////////////////////////////
/// Name: MenuButtons.cs
/// Description: Handles conrolling menu buttons for each menu.
//////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuButtonController : MonoBehaviour
{
    [SerializeField]
    protected List<MenuButtons> menuButtons;

    public InputAction navigateUp;

    public InputAction navigateDown;

    public InputAction buttonActivate;

    public AdjustedAudioSource navigateButtonSound;

    protected int buttonIndex = 0;

    protected float updateButtonsBuffer = 0.2f;

    protected float currentButtonsBuffer = 0;

    private void Awake()
    {
        UpdateSelectedButton(false);
    }

    private void OnEnable()
    {
        EnableButtons();

        navigateUp.canceled += Navigate_Up;
        navigateDown.canceled += Navigate_Down;
        buttonActivate.canceled += Select_Button;
    }

    private void OnDisable()
    {
        DisableButtons();
    }

    public void EnableButtons()
    {
        navigateUp.Enable();
        navigateDown.Enable();
        buttonActivate.Enable();
    }

    public void DisableButtons()
    {
        navigateUp.canceled -= Navigate_Up;
        navigateDown.canceled -= Navigate_Down;
        buttonActivate.canceled -= Select_Button;

        navigateUp.Disable();
        navigateDown.Disable();
        buttonActivate.Disable();
    }

    private void Navigate_Up(InputAction.CallbackContext obj)
    {
        ++buttonIndex;

        if (buttonIndex > menuButtons.Count - 1)
        {
            buttonIndex = 0;
        }

        currentButtonsBuffer = updateButtonsBuffer;

        UpdateSelectedButton();

        PlayButtonSound();
    }

    private void Navigate_Down(InputAction.CallbackContext obj)
    {
        --buttonIndex;

        if (buttonIndex < 0)
        {
            buttonIndex = menuButtons.Count - 1;
        }

        currentButtonsBuffer = updateButtonsBuffer;

        UpdateSelectedButton();

        PlayButtonSound();
    }

    private void Select_Button(InputAction.CallbackContext obj)
    {
        if (buttonIndex >= 0 && buttonIndex < menuButtons.Count)
        {
            menuButtons[buttonIndex].PressButton();
        }
        else
        {
            UnityEngine.Debug.LogError("Button index somehow got to: " + buttonIndex);
        }
    }

    protected void UpdateSelectedButton(bool playSounds = true)
    {
        for(int i = 0; i < menuButtons.Count; ++i)
        {
            if(i == buttonIndex)
            {
                menuButtons[i].Selected(playSounds);
            }
            else
            {
                menuButtons[i].NotSelected();
            }
        }
    }

    protected void PlayButtonSound()
    {
        if (SoundVolumeController.Instance)
        {
            SoundVolumeController.Instance.PlaySound(navigateButtonSound);
        }
        else
        {
            navigateButtonSound.Play();
        }
    }
}
