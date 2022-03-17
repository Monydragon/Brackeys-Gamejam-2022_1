using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystems : MonoBehaviour
{
    private static GameSystems _instance;
    public static GameSystems Instance { get {return _instance;} }

    // Managers
    public UIManager UIManager;
    public StateManager StateManager;
    public LevelSceneManager LevelSceneManager;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance);
        }
        else
        {
            _instance = this;
            InitSystems();
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Responsible for initializing all game systems
    /// </summary>
    private void InitSystems()
    {
        // Initialize Systems
        UIManager = new UIManager();
        StateManager = new StateManager(this);
        LevelSceneManager = new LevelSceneManager();

        // Start the game
        StartCoroutine(StateManager.NavigateToState(typeof(MainMenuState)));
    }
}
