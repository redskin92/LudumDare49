using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MenuButtonController
{
    public static PauseMenuController Instance { get; private set; }

    [SerializeField]
    protected GameObject pauseMenuBase;

    public InputAction togglePause;

    protected bool active = false;

    public event Action GamePaused;
    public event Action GameUnpaused;

    protected override void Awake()
    {
        base.Awake();

        Instance = this;
    }

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

            if (GamePaused != null)
                GamePaused();
        }
        else
        {
            UnRegisterButtons();
            pauseMenuBase.SetActive(false);

            if (GameUnpaused != null)
                GameUnpaused();
        }
    }
}
