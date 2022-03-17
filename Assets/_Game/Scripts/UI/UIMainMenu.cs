using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LoadStateOnTrigger;

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
        HealthComponent.ResetPlayerHealth();
        _systems.StateManager.NavigateToState(typeof(Forest1State),false, true);
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
    public void DEBUG_ToggleDisplayDebugMenu()
    {
        _debugMenuGroup.SetActive(!_debugMenuGroup.activeSelf);
    }

    public void DEBUG_JumpToState(string stateString)
    {

        GameState state = (GameState)Enum.Parse(typeof(GameState), stateString);
        switch(state)
        {
            case GameState.Forest1:
                _systems.StateManager.NavigateToState(typeof(Forest1State),false,true);
                break;
            case GameState.Forest2:
                _systems.StateManager.NavigateToState(typeof(Forest2State), false, true);
                break;
            case GameState.Tavern:
                _systems.StateManager.NavigateToState(typeof(TavernState), false, true);
                break;
            case GameState.GhostForest1:
                _systems.StateManager.NavigateToState(typeof(GhostForest1State), false, true);
                break;
            case GameState.GhostForest2:
                _systems.StateManager.NavigateToState(typeof(GhostForest2State), false, true);
                break;
            case GameState.Dungeon1:
                _systems.StateManager.NavigateToState(typeof(Dungeon1State), false, true);
                break;
            case GameState.Dungeon2:
                _systems.StateManager.NavigateToState(typeof(Dungeon2State), false, true);
                break;
            case GameState.Apartment1:
                _systems.StateManager.NavigateToState(typeof(Apartment1State), false, true);
                break;
            case GameState.Apartment2:
                _systems.StateManager.NavigateToState(typeof(Apartment2State), false, true);
                break;
            case GameState.Apartment3:
                _systems.StateManager.NavigateToState(typeof(Apartment3State), false, true);
                break;
        }
    }
#endif
}
