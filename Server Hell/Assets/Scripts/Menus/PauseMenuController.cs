using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MenuButtonController
{
    [SerializeField]
    protected GameObject pauseMenuBase;

    public InputAction togglePause;

    protected bool active = false;

    protected override void OnEnable()
    {
        EnableButtons();

        togglePause.Enable();

        togglePause.canceled += TogglePauseGame;

        pauseMenuBase.SetActive(active);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        togglePause.canceled -= TogglePauseGame;

        togglePause.Disable();
    }

    private void TogglePauseGame(InputAction.CallbackContext obj)
    {
        active = !active;

        if(active)
        {
            pauseMenuBase.SetActive(true);
            RegisterButtons();
        }
        else
        {
            UnRegisterButtons();
            pauseMenuBase.SetActive(false);
        }
    }
}
