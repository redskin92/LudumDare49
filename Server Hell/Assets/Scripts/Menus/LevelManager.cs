using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    protected BackgroundFadeManager backgroundFadeManager;

    [SerializeField]
    protected List<string> playScenesList;

    [SerializeField]
    protected string mainMenu;

    [SerializeField]
    protected float fadeTime = 1.0f;

    protected List<string> activeScenes;

    public event Action<LevelManager> TransitionComplete;

    bool loadMain = false;

    private static LevelManager instance = null;

    // Level Manager Singleton
    public static LevelManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;

        activeScenes = new List<string>();
    }

    void Start()
    {
        StartCoroutine("waitAFewFrames");
    }

    void Destory()
    {
        ForceUnloadAllScenes();
    }

    public void UnloadAllScenes()
    {
        string topSceneName = string.Empty;

        if(activeScenes.Count >= 1)
        {
            topSceneName = activeScenes[0];
        }

        ForceUnloadAllScenes();

        StartCoroutine("waitForSceneUnLoad", topSceneName);
    }

    private void ForceUnloadAllScenes()
    {
        for (int i = 0; i < activeScenes.Count; ++i)
        {
            UnloadSceneSafe(activeScenes[i]);
        }

        activeScenes.Clear();
    }

    /// <summary>
    /// Load in the initial scenes
    /// </summary>
    public void LoadInitialScenes()
    {
        loadMain = true;

        StartCoroutine("waitForSceneLoad", mainMenu);

        LoadSceneSafe(mainMenu);
    }

    public void TransitionToMain()
    {
        loadMain = true;
        StartTransition();
    }

    public void TransitionToPlay()
    {
        loadMain = false;
        StartTransition();
    }

    private void LoadInMainMenu()
    {
        LoadSceneSafe(mainMenu);

        StartCoroutine("waitForSceneLoad", mainMenu);
    }

    private void LoadInPlay()
    {
        string topSceneName = string.Empty;

        if (playScenesList.Count >= 1)
        {
            topSceneName = playScenesList[0];
        }

        for (int i = 0; i < playScenesList.Count; ++i)
        {
            LoadSceneSafe(playScenesList[i]);
        }

        StartCoroutine("waitForSceneLoad", topSceneName);
    }

    /// <summary>
    /// Make sure a scene is unloaded before trying to load it. LOAD!
    /// </summary>
    /// <param name="sceneName"></param>
    private void LoadSceneSafe(string sceneName)
    {
        Scene lookupScene = SceneManager.GetSceneByName(sceneName);

        if (!lookupScene.isLoaded && !activeScenes.Contains(sceneName))
        {
            activeScenes.Add(sceneName);
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }

    /// <summary>
    /// Make sure a scene is loaded before trying to unload it. LOAD!
    /// </summary>
    /// <param name="sceneName"></param>
    private void UnloadSceneSafe(string sceneName)
    {
        Scene lookupScene = SceneManager.GetSceneByName(sceneName);

        if (!string.IsNullOrEmpty(lookupScene.name) && lookupScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(lookupScene);
        }
    }

    /// <summary>
    /// When the fade in has completed, unload the selected level
    /// </summary>
    /// <param name="manager"></param>
    private void FadeInCompleted(BackgroundFadeManager manager)
    {
        backgroundFadeManager.FadeInComplete -= FadeInCompleted;

        UnloadAllScenes();
    }

    /// <summary>
    /// When the fader out has completed, let one of our levels know it can start
    /// </summary>
    /// <param name="manager"></param>
    private void FadeOutCompleted(BackgroundFadeManager manager)
    {
        backgroundFadeManager.FadeOutComplete -= FadeOutCompleted;

        TransitionComplete?.Invoke(this);
    }

    protected void StartTransition()
    {
        backgroundFadeManager.FadeInComplete += FadeInCompleted;
        backgroundFadeManager.SetTimer(fadeTime, true);
    }

    IEnumerator waitForSceneLoad(string sceneName)
    {
        while (!string.IsNullOrEmpty(SceneManager.GetSceneByName(sceneName).name) && SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            yield return null;
        }

        backgroundFadeManager.FadeOutComplete += FadeOutCompleted;
        backgroundFadeManager.SetTimer(fadeTime, false);
    }

    IEnumerator waitForSceneUnLoad(string sceneName)
    {
        while (!string.IsNullOrEmpty(SceneManager.GetSceneByName(sceneName).name) && !SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            yield return null;
        }

        if (loadMain)
        {
            LoadInMainMenu();
        }
        else
        {
            LoadInPlay();
        }
    }

    IEnumerator waitAFewFrames()
    {
        yield return new WaitForSeconds(0.5f);

        LoadInitialScenes();
    }

}
