using UnityEngine;

public class ButtonTutorial : MenuButtons
{
    [SerializeField]
    protected AdjustedAudioSource mainMenuMusic;

    /// <summary>
    /// Start the tutorial
    /// </summary>
    public override void PressButton()
    {
        mainMenuMusic.FadeSound();

        PlayPressedSound();
        LevelManager.Instance.TransitionToTutorial();
    }
}
