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
    protected string playSceneName;

    [SerializeField]
    protected string mainMenu;

    [SerializeField]
    protected float fadeTime = 1.0f;

    [SerializeField]
    protected AudioListener listener;

    protected List<string> activeScenes;

    public event Action<LevelManager> TransitionComplete;

    bool loadMain = false;

    string currentScene = string.Empty;

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

        currentScene = mainMenu;

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
        currentScene = mainMenu;

        LoadSceneSafe(mainMenu);

        StartCoroutine("waitForSceneLoad", mainMenu);
    }

    private void LoadInPlay()
    {
        currentScene = playSceneName;

        LoadSceneSafe(playSceneName);

        StartCoroutine("waitForSceneLoad", playSceneName);
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

        if(currentScene.Equals(playSceneName))
        {
            UnityEngine.Debug.Log("LoadIn");
            listener.enabled = false;
        }
        if (currentScene.Equals(mainMenu))
        {
            listener.enabled = true;
        }
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
