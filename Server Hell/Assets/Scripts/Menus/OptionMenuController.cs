using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OptionMenuController : MenuButtonController
{
    [SerializeField]
    protected GameObject optionsMenuBase;

    public InputAction lowerVolume;

    public InputAction raiseVolume;

    protected int currentVolumeState = 0;

    protected MenuButtonController baseMenuButtonController = null;

    public InputAction backOut;

    [SerializeField]
    protected MenuButtons exitOptionsButton;

    protected bool optionsActive = false;

    protected override void OnEnable()
    {
        EnableButtons();
    }

    protected override void Start()
    {
        base.Start();

        optionsMenuBase.SetActive(optionsActive);
    }

    private void FixedUpdate()
    {
        ButtonAdjustVolume adjustVolume = menuButtons[buttonIndex].GetComponent<ButtonAdjustVolume>();

        if (adjustVolume)
        {
            switch (currentVolumeState)
            {
                case -1:
                    adjustVolume.LowerVolume();
                    break;
                case 0:
                    break;
                case 1:
                    adjustVolume.RaiseVolume();
                    break;
            }
        }
    }

    public void EnableOptionsMenu(MenuButtonController buttonController)
    {
        // disable the base buttons under the options menu
        buttonController.UnRegisterButtons();

        baseMenuButtonController = buttonController;

        optionsMenuBase.SetActive(true);

        optionsActive = true;

        buttonIndex = 0;

        UpdateSelectedButton();

        RegisterButtons();
    }

    public void DisableOptionsMenu()
    {
        UnRegisterButtons();

        buttonIndex = 0;

        optionsMenuBase.SetActive(false);

        optionsActive = false;

        if (baseMenuButtonController)
        {
            baseMenuButtonController.RegisterButtons();
            baseMenuButtonController = null;
        }
    }

    public override void EnableButtons()
    {
        base.EnableButtons();

        lowerVolume.Enable();
        raiseVolume.Enable();
        backOut.Enable();
    }

    public override void RegisterButtons()
    {
        base.RegisterButtons();

        lowerVolume.started += Lower_Volume;
        raiseVolume.started += Raise_Volume;

        backOut.started += BackOut;
    }

    public override void UnRegisterButtons()
    {
        base.UnRegisterButtons();

        lowerVolume.started -= Lower_Volume;
        raiseVolume.started -= Raise_Volume;

        backOut.started -= BackOut;
    }

    protected override void UpdateSelectedButton(bool playSounds = true)
    {
        base.UpdateSelectedButton(playSounds);

        currentVolumeState = 0;
    }

    private void Raise_Volume(InputAction.CallbackContext obj)
    {
        ButtonAdjustVolume adjustVolume = menuButtons[buttonIndex].GetComponent<ButtonAdjustVolume>();

        if (adjustVolume)
        {
            currentVolumeState = 1;
            raiseVolume.canceled += StopAdjustingVolume;
        }
    }

    private void Lower_Volume(InputAction.CallbackContext obj)
    {
        ButtonAdjustVolume adjustVolume = menuButtons[buttonIndex].GetComponent<ButtonAdjustVolume>();

        if (adjustVolume)
        {
            currentVolumeState = -1;
            lowerVolume.canceled += StopAdjustingVolume;
        }
    }

    private void BackOut(InputAction.CallbackContext obj)
    {
        if(exitOptionsButton && optionsActive)
        {
            UnRegisterButtons();

            exitOptionsButton.PressButton();
        }
    }

    private void StopAdjustingVolume(InputAction.CallbackContext obj)
    {
        currentVolumeState = 0;
        raiseVolume.canceled -= StopAdjustingVolume;
        lowerVolume.canceled -= StopAdjustingVolume;
    }
}
