using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;
using System.Linq;
public class LoadStateOnTrigger : MonoBehaviour
{
    public bool hasCondition;
    public int allowedEnemiesToContinue;
    public int enemiesRemainingonMap;
    public GameState stateToChange;
    public Flowchart flowchart;
    public BlockReference startingBlock;
    private List<GameObject> enemies;

    private void OnEnable()
    {
        EventManager.onObjectDied += EventManager_onObjectDied;
    }

    private void EventManager_onObjectDied(GameObject _value)
    {
        if (enemies.Find(x=> x == _value))
        {
            enemiesRemainingonMap = GameObject.FindGameObjectsWithTag("Enemy").Length;
            enemiesRemainingonMap--;
        }

    }

    private void OnDisable()
    {
        EventManager.onObjectDied -= EventManager_onObjectDied;
    }
    private void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        enemiesRemainingonMap = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(hasCondition && enemiesRemainingonMap <= allowedEnemiesToContinue)
            {
                ChangeState(stateToChange);
            }
            else if(hasCondition)
            {
                if(flowchart != null && startingBlock.block != null)
                {
                    flowchart.ExecuteBlock(startingBlock.block);
                }
            }
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
