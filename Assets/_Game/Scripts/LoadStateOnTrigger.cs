using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadStateOnTrigger : MonoBehaviour
{
    public enum GameState
    {
        Apartment1,
        Apartment2,
        Apartment3,
        Forest1,
        Forest2,
        GhostForest1,
        GhostForest2,
        Dungeon1,
        Dungeon2,
        Tavern,
        Credits,
        Gameover,
        Mainmenu
    }
    public GameState stateToChange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ChangeState(stateToChange);
        }
    }

    public void ChangeState(GameState _stateToChange)
    {
        switch (_stateToChange)
        {
            case GameState.Apartment1:
                GameSystems.Instance.StateManager.NavigateToState(typeof(Apartment1State));

                break;
            case GameState.Apartment2:
                GameSystems.Instance.StateManager.NavigateToState(typeof(Apartment2State));
                break;
            case GameState.Apartment3:
                GameSystems.Instance.StateManager.NavigateToState(typeof(Apartment3State));
                break;
            case GameState.Forest1:
                GameSystems.Instance.StateManager.NavigateToState(typeof(Forest1State));
                break;
            case GameState.Forest2:
                GameSystems.Instance.StateManager.NavigateToState(typeof(Forest2State));
                break;
            case GameState.GhostForest1:
                GameSystems.Instance.StateManager.NavigateToState(typeof(GhostForest1State));
                break;
            case GameState.GhostForest2:
                GameSystems.Instance.StateManager.NavigateToState(typeof(GhostForest2State));
                break;
            case GameState.Dungeon1:
                GameSystems.Instance.StateManager.NavigateToState(typeof(Dungeon1State));
                break;
            case GameState.Dungeon2:
                GameSystems.Instance.StateManager.NavigateToState(typeof(Dungeon2State));
                break;
            case GameState.Tavern:
                GameSystems.Instance.StateManager.NavigateToState(typeof(TavernState));
                break;
            case GameState.Credits:
                GameSystems.Instance.StateManager.NavigateToState(typeof(CreditsState));
                break;
            case GameState.Gameover:
                GameSystems.Instance.StateManager.NavigateToState(typeof(GameOverState));
                break;
            case GameState.Mainmenu:
                GameSystems.Instance.StateManager.NavigateToState(typeof(MainMenuState));
                break;
        }
    }
}
