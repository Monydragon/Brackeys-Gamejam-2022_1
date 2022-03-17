using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICredits : MonoBehaviour
{
    private GameSystems _systems;
    public void Setup(GameSystems systems)
    {
        _systems = systems;
    }

    public void OnBackClicked()
    {
        StartCoroutine(_systems.StateManager.NavigateToState(null, true));
    }
}
