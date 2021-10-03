using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniMapController : MonoBehaviour
{
    [SerializeField]
    protected GameObject miniMapBase;

    public InputAction toggleMiniMap;

    protected bool active = false;

    void OnEnable()
    {
        miniMapBase.SetActive(active);

        toggleMiniMap.Enable();

        toggleMiniMap.canceled += ToggleMiniMap;
    }

    void OnDisable()
    {
        toggleMiniMap.canceled -= ToggleMiniMap;

        toggleMiniMap.Disable();
    }

    private void ToggleMiniMap(InputAction.CallbackContext obj)
    {
        active = !active;

        miniMapBase.SetActive(active);
    }
}
