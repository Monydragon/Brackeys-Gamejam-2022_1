using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    private GameSystems _systems;

    [SerializeField] private GameObject _quitButton;

    public void Setup(GameSystems systems)
    {
        _systems = systems;

#if UNITY_WEBGL
        _quitButton?.SetActive(false);
#endif
    }

    public void OnRetryClicked()
    {
        _systems.StateManager.NavigateToState(null, true);
        EventManager.ResetCurrentLevel();
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
