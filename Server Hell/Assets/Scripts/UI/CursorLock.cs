using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLock : MonoBehaviour
{
    [SerializeField]
    private bool cursorLock = true;

    private void Start()
    {
        if (cursorLock)
            Cursor.lockState = CursorLockMode.Confined;
    }
}
