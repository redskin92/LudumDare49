//////////////////////////////////////////////////////////////////////////////////////////
/// Name: BackgroundFadeManager.cs
/// Description: Handles fading in and out the screen for transitions.
//////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class BackgroundFadeManager : MonoBehaviour
{
    [SerializeField]
    protected Image fadeImage;

    protected float fadeTime = 1f;

    float currentFadeTime = 0f;

    bool fadeDone = false;

    bool fadeIn = false;

    public event Action<BackgroundFadeManager> FadeInComplete;

    public event Action<BackgroundFadeManager> FadeOutComplete;

    private void Awake()
    {
        // set to fade in from the start
        currentFadeTime = fadeTime;
    }

    // fade out the screen whenever needed
    void Update ()
    {
        if (currentFadeTime > 0)
        {
            fadeImage.enabled = true;

            if(Time.unscaledDeltaTime < 1.0f)
            {
                currentFadeTime -= Time.unscaledDeltaTime;
            }

            Color color = Color.black;

            if (fadeIn)
            {
                color.a = 1.0f - (currentFadeTime / fadeTime);
            }
            else
            {
                color.a = currentFadeTime / fadeTime;
            }

            fadeImage.color = color;
        }
        else
        {
            if (!fadeDone)
            {
                fadeDone = true;

                if(fadeIn)
                {

                    FadeInComplete?.Invoke(this);
                }
                else
                {
                    FadeOutComplete?.Invoke(this);
                }
            }
            if (!fadeIn)
            {
                fadeImage.enabled = false;
            }
        }
    }

    // reset fade variables
    public void SetTimer(float timer, bool _fadeIn)
    {
        fadeDone = false;
        fadeTime = timer;
        currentFadeTime = timer;
        fadeIn = _fadeIn;
    }
}
