using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    enum EnemyAIState { Idle, Attacking }

    public float AggroRange = 3f;
    public float RangeToAttack = .5f;
    public float WanderRadius = 1f;
    public float WanderTargetAccuracy = .1f;
    public float WanderIdleTime = 1f;

    EnemyAIState CurrentState = EnemyAIState.Idle;
    Vector2 SpawnLocation;
    Vector2 TargetLocation;
    EnemyMovement MovementComponent;
    GameObject PlayerGameObject;
    bool WaitingForNewWanderTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize PlayerGameObject
        PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
        //Initialize SpawnLocation
        SpawnLocation = gameObject.transform.position;
        //Initialize TargetLocation
        TargetLocation = SpawnLocation + (Random.insideUnitCircle*WanderRadius);
        //Initialize Movement
        MovementComponent = GetComponent<EnemyMovement>();
        if(MovementComponent == null)
        {
            Debug.LogError("EnemyAI: No Movement Component on GameObject: " + gameObject.name);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //If not attacking, but player is in range
        if (CurrentState != EnemyAIState.Attacking && IsPlayerInAggroRange())
        {
            CurrentState = EnemyAIState.Attacking;
        }

        switch (CurrentState)
        {
            case EnemyAIState.Idle:
                Idle();
                break;
            case EnemyAIState.Attacking:

                break;
            default:
                break;
        }
    }
    void Idle()
    {
        if (WaitingForNewWanderTarget)
        {
            return;
        }
        Vector2 position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        Vector2 targetLocationDirection = TargetLocation - position;
        float distanceToTarget= targetLocationDirection.magnitude;
        MovementComponent.Move(targetLocationDirection);
        if (distanceToTarget < WanderTargetAccuracy)
        {
            StartCoroutine(WaitThenSetNewWanderLocation());
            WaitingForNewWanderTarget = true;
        }

    }
    bool IsPlayerInAggroRange()
    {
        if (PlayerGameObject == null) return false;
        // get normalized direction of player
        Vector2 playerDirection = PlayerGameObject.transform.position - gameObject.transform.position;
        playerDirection.Normalize();
        //raycast in that direction at aggro range distance
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, playerDirection, AggroRange);
        if(hit.collider != null)
        {
            //if it was the player we hit (Not a wall or something else) return true
            if(hit.collider.gameObject == PlayerGameObject)
            {
                return true;
            }
        }
        //otherwise
        return false;

    }
    IEnumerator WaitThenSetNewWanderLocation()
    {
        yield return new WaitForSeconds(WanderIdleTime);
        TargetLocation = SpawnLocation + (Random.insideUnitCircle * WanderRadius);
        WaitingForNewWanderTarget = false;
    }
}
