using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyParentText : MonoBehaviour
{
    [SerializeField]
    protected Text self;

    [SerializeField]
    protected Text parent;

    void FixedUpdate()
    {
        if(parent)
        {
            self.text = parent.text;
        }
    }
}
