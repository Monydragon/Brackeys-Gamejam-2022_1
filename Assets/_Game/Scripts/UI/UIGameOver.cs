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
        // TODO: Hook into retry/checkpoint mechanic
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
