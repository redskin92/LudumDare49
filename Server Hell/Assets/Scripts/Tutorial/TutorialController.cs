using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialController : MonoBehaviour
{
    public PlayerInput playerInputMain;

    // Start is called before the first frame update
    void Awake()
    {
        playerInputMain.actions.Disable();
    }

    private void Start()
    {
        playerInputMain.actions.Enable();
    }
}
