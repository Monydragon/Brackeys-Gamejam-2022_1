using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    private GameSystems _systems;
    [SerializeField] private GameObject _quitButton;

    public void Setup(GameSystems systems)
    {
        _systems = systems;

        // Disable quit button on WebGL builds
#if UNITY_WEBGL
        _quitButton?.SetActive(false);
#endif
    }

    public void OnStartClicked()
    {
        _systems.StateManager.NavigateToState(typeof(Forest1State));
    }

    public void OnSettingsClicked()
    {
        _systems.StateManager.NavigateToState(typeof(SettingsState));
    }

    public void OnCreditsClicked()
    {
        _systems.StateManager.NavigateToState(typeof(CreditsState));
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }

#if DEBUG
    [SerializeField] private GameObject _debugMenuGroup;
    public enum JumpState
    {
        Forest1State,
        Forest2State,
        TavernState,
        GhostForest1State,
        GhostForest2State,
        Dungeon1State,
        Dungeon2State,
        Apartment1State,
        Apartment2State,
        Apartment3State,
    }
    public void DEBUG_ToggleDisplayDebugMenu()
    {
        _debugMenuGroup.SetActive(!_debugMenuGroup.activeSelf);
    }

    public void DEBUG_JumpToState(string stateString)
    {
        JumpState state = (JumpState)Enum.Parse(typeof(JumpState), stateString);
        switch(state)
        {
            case JumpState.Forest1State:
                _systems.StateManager.NavigateToState(typeof(Forest1State));
                break;
            case JumpState.Forest2State:
                _systems.StateManager.NavigateToState(typeof(Forest2State));
                break;
            case JumpState.TavernState:
                _systems.StateManager.NavigateToState(typeof(TavernState));
                break;
            case JumpState.GhostForest1State:
                //_systems.StateManager.NavigateToState(typeof());
                break;
            case JumpState.GhostForest2State:
                //_systems.StateManager.NavigateToState(typeof());
                break;
            case JumpState.Dungeon1State:
                //_systems.StateManager.NavigateToState(typeof());
                break;
            case JumpState.Dungeon2State:
                //_systems.StateManager.NavigateToState(typeof());
                break;
            case JumpState.Apartment1State:
                _systems.StateManager.NavigateToState(typeof(Apartment1State));
                break;
            case JumpState.Apartment2State:
                _systems.StateManager.NavigateToState(typeof(Apartment2State));
                break;
            case JumpState.Apartment3State:
                _systems.StateManager.NavigateToState(typeof(Apartment3State));
                break;
        }
    }
#endif
}
