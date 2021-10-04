using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPause : MonoBehaviour
{
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        if (PauseMenuController.Instance != null)
        {
            PauseMenuController.Instance.GamePaused += PauseMenu_GamePaused;
            PauseMenuController.Instance.GameUnpaused += PauseMenu_GameUnpaused;
        }
    }

    private void OnDestroy()
    {
        if (PauseMenuController.Instance != null)
        {
            PauseMenuController.Instance.GamePaused -= PauseMenu_GamePaused;
            PauseMenuController.Instance.GameUnpaused -= PauseMenu_GameUnpaused;
        }
    }

    private void PauseMenu_GamePaused()
    {
        playerInput.actions.Disable();
    }

    private void PauseMenu_GameUnpaused()
    {
        playerInput.actions.Enable();
    }
}
