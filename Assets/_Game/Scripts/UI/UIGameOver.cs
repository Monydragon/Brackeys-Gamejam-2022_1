using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    private GameSystems _systems;

    public void Setup(GameSystems systems)
    {
        _systems = systems;
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
