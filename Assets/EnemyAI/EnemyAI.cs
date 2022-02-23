using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    enum EnemyAIState { Idle, AttackingPlayer }

    public float aggroRange = 3f;
    public float deAggroRange = 4f;
    public float rangeToAttack = .5f;
    public float wanderRadius = 1f;
    public float wanderTargetAccuracy = .1f;
    public float wanderMinimumIdleTime = 1f;
    public float wanderMaximumIdleTime = 2f;
    public LayerMask ObstacleLayerMask;

    private EnemyAIState currentState = EnemyAIState.Idle;
    private Vector2 spawnLocation;
    private GameObject wanderTargetObject;
    private GameObject playerGameObject;
    private bool waitingForNewWanderTarget = false;
    private AIDestinationSetter destinationSetter;
    private AIPath aiPath;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize PlayerGameObject
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        //Initialize SpawnLocation
        spawnLocation = gameObject.transform.position;
        //Initialize targetObject
        wanderTargetObject = new GameObject(gameObject.name + "_" + "TargetObject");
        //Initialize destinationSetter
        destinationSetter = GetComponent<AIDestinationSetter>();
        //Initialize aiPath
        aiPath = GetComponent<AIPath>();

        //print warnings for nulls

        if (aiPath == null)
        {
            Debug.LogError("EnemyAI: No AIPath on GameObject: " + gameObject.name);
        }
        if (destinationSetter == null)
        {
            Debug.LogError("EnemyAI: No AIDestinationSetter on GameObject: " + gameObject.name);
            return;
        }
        destinationSetter.target = wanderTargetObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Pre Action
        //Check if should switch state
        switch (currentState)
        {
            case EnemyAIState.Idle:
                if (ShouldAggroToPlayer())
                {
                    currentState = EnemyAIState.AttackingPlayer;
                    destinationSetter.target = playerGameObject.transform;
                }
                break;
            case EnemyAIState.AttackingPlayer:
                if (!IsPlayerInRange(deAggroRange))
                {
                    currentState = EnemyAIState.Idle;
                    destinationSetter.target = wanderTargetObject.transform;
                }
                break;
            default:
                break;
        }
        //Action
        //Take action based on state
        switch (currentState)
        {
            case EnemyAIState.Idle:
                Idle();
                break;
            case EnemyAIState.AttackingPlayer:
                if (IsPlayerInRange(rangeToAttack))
                {
                    AttackPlayer();

                }
                break;
            default:
                break;
        }
    }
    void AttackPlayer()
    {
        Debug.Log("Attacked Player");
    }
    void Idle()
    {
        if (aiPath.reachedEndOfPath && !waitingForNewWanderTarget)
        {
            waitingForNewWanderTarget = true;
            StartCoroutine(WaitThenSetNewWanderLocation());

        }
    }
    
    bool ShouldAggroToPlayer()
    {
        if(IsPlayerInRange(aggroRange) && PlayerInLOS()) 
        { 
            return true; 
        }else 
        { 
            return false; 
        }
       
    }
    private bool IsPlayerInRange(float _range)
    {
        if (playerGameObject == null) return false;
        // get normalized direction of player
        Vector2 playerDirection = playerGameObject.transform.position - gameObject.transform.position;
        if (playerDirection.magnitude > _range) { return false; }
        return true;
    }
    private bool PlayerInLOS()
    {
        //return true if no player
        if (playerGameObject == null) return false;
        Vector2 playerDirection = playerGameObject.transform.position - gameObject.transform.position;
        float playerDistance = playerDirection.magnitude;
        playerDirection.Normalize();

        //raycast in the player direction to the distance of the player looking for obstacles
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, playerDirection, playerDistance, ObstacleLayerMask);
        if (hit.collider != null) // if we hit anything LOS is blocked
        {
            return false;
        }
        //otherwise
        return true;
    }
    IEnumerator WaitThenSetNewWanderLocation()
    {
        float idleTime = Random.Range(wanderMinimumIdleTime, wanderMaximumIdleTime);
        yield return new WaitForSeconds(idleTime);
        wanderTargetObject.transform.position = spawnLocation + (Random.insideUnitCircle * wanderRadius);
        //Needed to wait for aiPath to realize it's no longer at end of path
        //TODO: probably make this a variable later if no better solution
        yield return new WaitForSeconds(.5f);
        waitingForNewWanderTarget = false;
    }
}
