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
}
