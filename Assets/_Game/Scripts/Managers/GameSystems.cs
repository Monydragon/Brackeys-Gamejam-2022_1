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
        UIManager = new UIManager();
        StateManager = new StateManager(this);

        //TODO: Replace with main menu transition
        StateManager.NavigateToState(typeof(Forest1State));
    }
}
