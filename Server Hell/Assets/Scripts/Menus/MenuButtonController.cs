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

    [SerializeField]
    protected GameObject controlsScemeBase;

    public InputAction navigateUp;

    public InputAction navigateDown;

    public InputAction buttonActivate;

    public AdjustedAudioSource navigateButtonSound;

    protected int buttonIndex = 0;

    protected virtual void Awake()
    {
        UpdateSelectedButton(false);
    }

    protected virtual void OnEnable()
    {
        EnableButtons();

        RegisterButtons();
    }

    protected virtual void OnDisable()
    {
        DisableButtons();

        StopCoroutine("StartInteractionSequence");
    }

    public virtual void EnableButtons()
    {
        navigateUp.Enable();
        navigateDown.Enable();
        buttonActivate.Enable();
    }

    public virtual void RegisterButtons()
    {
        navigateUp.canceled += Navigate_Up;
        navigateDown.canceled += Navigate_Down;

        if (controlsScemeBase)
        {
            controlsScemeBase.SetActive(true);
        }

        DelayButtonPressed();
    }

    public virtual void UnRegisterButtons()
    {
        navigateUp.canceled -= Navigate_Up;
        navigateDown.canceled -= Navigate_Down;
        buttonActivate.canceled -= Select_Button;

        if (controlsScemeBase)
        {
            controlsScemeBase.SetActive(false);
        }
    }

    public virtual void DisableButtons()
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
        --buttonIndex;

        if (buttonIndex < 0)
        {
            buttonIndex = menuButtons.Count - 1;
        }

        UpdateSelectedButton();

        PlayButtonSound();
    }

    private void Navigate_Down(InputAction.CallbackContext obj)
    {
        ++buttonIndex;

        if (buttonIndex > menuButtons.Count - 1)
        {
            buttonIndex = 0;
        }

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

    protected virtual void UpdateSelectedButton(bool playSounds = true)
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

    public void DelayButtonPressed()
    {
        StartCoroutine("StartInteractionSequence");
    }

    private IEnumerator StartInteractionSequence()
    {
        yield return new WaitForSeconds(0.1f);

        buttonActivate.canceled += Select_Button;
    }
}
