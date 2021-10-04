using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLight : MonoBehaviour
{
    [SerializeField]
    protected Image imageToFade;

    [SerializeField]
    protected float endAlpha = 1.0f;

    [SerializeField]
    protected float endSize = 1.0f;

    [SerializeField]
    protected float maxWaitStartFade = 5.0f;

    [SerializeField]
    protected float minWaitStartFade = 2.5f;

    [SerializeField]
    protected float fadeInTime = 0.5f;

    [SerializeField]
    protected float fadeOutTime = 1.0f;

    protected float currentAlpha;

    protected float startingSize;

    protected float startingAlpha;

    protected float currentSize;

    protected float currentTime;

    protected bool waitToFade = true;

    protected bool fadingIn = false;

    private void Awake()
    {
        // this assumes the object is square
        startingSize = transform.localScale.x;

        ResetStartingVariables();

        if(imageToFade)
            startingAlpha = imageToFade.color.a;

        SetCurrentAlpha();
    }

    void FixedUpdate()
    {
        if (waitToFade)
        {
            currentTime -= Time.deltaTime;

            if(currentTime <= 0)
            {
                waitToFade = false;
                fadingIn = true;
                currentTime = 0;
            }
        }
        else
        {
            if (fadingIn)
            {
                currentTime += Time.deltaTime;

                if (currentTime >= fadeInTime)
                {
                    currentAlpha = endAlpha;
                    currentSize = endSize;
                    fadingIn = false;
                    currentTime = fadeOutTime;
                    SetCurrentAlpha();
                    SetSize();
                    return;
                }

                currentAlpha = Mathf.Lerp(startingAlpha, endAlpha, currentTime / fadeInTime);
                currentSize = Mathf.Lerp(endAlpha, endSize, currentTime / fadeInTime);
            }
            else
            {
                currentTime -= Time.deltaTime;

                if (currentTime <= 0)
                {
                    currentTime = 0;
                    currentAlpha = startingAlpha;
                    currentSize = startingSize;
                    currentTime = 0;
                    SetCurrentAlpha();
                    SetSize();
                    ResetStartingVariables();
                    return;
                }

                currentAlpha = Mathf.Lerp(startingAlpha, endAlpha, currentTime / fadeOutTime);
                currentSize = Mathf.Lerp(endAlpha, endSize, currentTime / fadeOutTime);
            }



            SetCurrentAlpha();
            SetSize();
        }

    }

    protected void SetCurrentAlpha()
    {
        if(imageToFade)
            imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, currentAlpha);
    }

    protected void SetSize()
    {
        transform.localScale = new Vector3(currentSize, currentSize, transform.localScale.z);
    }

    protected void ResetStartingVariables()
    {
        waitToFade = true;

        fadingIn = false;

        currentTime = Random.Range(minWaitStartFade, maxWaitStartFade);
    }
}
