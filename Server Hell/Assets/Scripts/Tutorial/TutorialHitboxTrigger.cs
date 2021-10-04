using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialHitboxTrigger : MonoBehaviour
{
    public UnityEvent triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            triggered.Invoke();

            Destroy(this);
        }
    }
}
