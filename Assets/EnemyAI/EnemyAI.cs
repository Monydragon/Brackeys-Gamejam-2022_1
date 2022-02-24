using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    enum EnemyAIState { Idle, AttackingPlayer }
    [Header("Aggro Settings")]
    [Tooltip("Distance the Player will aggro the enemy")]
    public float aggroRange = 3f;
    [Tooltip("Distance the Enemy will lose aggro and return to wandering")]
    public float deAggroRange = 4f;
    [Space(5)]

    [Header("Attack Settings")]
    [Tooltip("The minimum range to the player, before the enemy will try to perform an attack")]
    public float rangeToAttackPlayer = 1.3f;
    [Tooltip("The distance to spawn the hit box for the enemies attack")]
    public float attackBoxDistance = 1f;
    [Tooltip("The size of the hitbox")]
    public Vector2 attackBoxSize = new Vector2(1, 1);
    [Tooltip("The time to delay the damage of the attack once it has started")]
    public float attackDamageDelay = .5f;
    [Tooltip("The damage to deal when the attack hits the player")]
    public int attackDamage = 1;
    [Tooltip("The length of the attack animation")]
    public float attackAnimationLength = 1f;
    [Tooltip("The time before the enemy will attack again")]
    public float attackCooldown = 1.2f;
    [Space(5)]

    [Header("Wander Settings")]
    [Tooltip("The range from the Enemies spawn that he may set a wander target within")]
    public float wanderRadius = 5f;
    [Tooltip("The minimum time the enemy will wait once reaching it's wander destination")]
    public float wanderMinimumIdleTime = 1f;
    [Tooltip("The maximum time the enemy will wait once reaching it's wander destination")]
    public float wanderMaximumIdleTime = 2f;
    [Space(5)]

    [Header("Obstacles")]
    public LayerMask ObstacleLayerMask;

    private EnemyAIState currentState = EnemyAIState.Idle;
    private Vector2 spawnLocation;
    private bool currentlyAttacking = false;
    private bool attackOnCooldown = false;
    private GameObject wanderTargetObject;
    private GameObject playerGameObject;
    private bool waitingForNewWanderTarget = false;
    private AIDestinationSetter destinationSetter;
    private AIPath aiPath;
    private Rigidbody2D rb;
    

    // Start is called before the first frame update
    void Start()
    {
        //Initialize Rigidbody
        rb = GetComponent<Rigidbody2D>();
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

    private void OnEnable()
    {
        EventManager.onObjectDied += OnObjectDied;
        EventManager.onDamageActor += OnDamageActor;
    }
    private void OnDisable()
    {
        EventManager.onObjectDied -= OnObjectDied;
        EventManager.onDamageActor -= OnDamageActor;
    }
    private void OnObjectDied(GameObject objectThatDied)
    {
        if(objectThatDied == gameObject)
        {
            StopAllCoroutines();
            aiPath.canMove = false;
        }
    }
    private void OnDamageActor(GameObject _target, GameObject _attacker, int _dmg, float _knockback)
    {
        if(_target == gameObject)
        {
            Vector2 direction = (transform.position-_attacker.transform.position).normalized;
            rb.AddForce(direction * _knockback);
        }
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
                    StopCoroutine("WaitThenSetNewWanderLocation");
                    aiPath.canMove = true;
                    currentState = EnemyAIState.AttackingPlayer;
                    destinationSetter.target = playerGameObject.transform;
                }
                break;
            case EnemyAIState.AttackingPlayer:
                if (GetDistanceToPlayer() > deAggroRange)//if player has gone further than deaggro range
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
                Wander();
                break;
            case EnemyAIState.AttackingPlayer:
                if (GetDistanceToPlayer() < rangeToAttackPlayer)
                {
                    TryToAttack();
                }
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if collided with other Enemy while idle, wait and find a new wander location
        //TODO: This is stupid and I hate it. There's a better way. -the man who wrote the code
        if (currentState == EnemyAIState.Idle && collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(WaitThenSetNewWanderLocation());
        }
    }

    void TryToAttack()
    {
        if (!currentlyAttacking && !attackOnCooldown)
        {
            //Perform Attack
            StartCoroutine(AttackCooldown());
            StartCoroutine(StopMovingAndPerformAttackAnimation());
            StartCoroutine(DelayAndDealAttackDamage());
        }
    }

    //Gets the direction the character is facing(Up, Down, Left, Right)
    private Vector2 GetDirectionFacing()
    {
        //get direction from 4 cardinal directions
        Vector2 direction = aiPath.desiredVelocity.normalized;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            direction = new Vector2(direction.x, 0);
            direction.Normalize();
        }
        else
        {
            direction = new Vector2(0, direction.y);
            direction.Normalize();
        }

        return direction;
    }

    //If AI has reached the end of its current wandering path, wait for a new one
    void Wander()
    {
        if (aiPath.reachedEndOfPath && !waitingForNewWanderTarget)
        {
            waitingForNewWanderTarget = true;
            StartCoroutine(WaitThenSetNewWanderLocation());

        }
    }
    
    //If player is in Line of Sight and distance is less than aggro range
    bool ShouldAggroToPlayer()
    {
        if(GetDistanceToPlayer() < aggroRange && PlayerInLOS()) 
        { 
            return true; 
        }else 
        { 
            return false; 
        }
       
    }

    //returns distance to player
    private float GetDistanceToPlayer()
    {
        if (playerGameObject == null) return float.NaN;
        // get normalized direction of player
        Vector2 playerDirection = playerGameObject.transform.position - gameObject.transform.position;
        return playerDirection.magnitude;
    }

    //returns true if there is nothing blocking a raycast between player and enemy on ObstacleLayerMask
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

    //waits then sets a new Wander Target
    private IEnumerator WaitThenSetNewWanderLocation()
    {
        rb.velocity = Vector3.zero;
        aiPath.canMove = false;
        float idleTime = Random.Range(wanderMinimumIdleTime, wanderMaximumIdleTime);
        yield return new WaitForSeconds(idleTime);
        wanderTargetObject.transform.position = spawnLocation + (Random.insideUnitCircle * wanderRadius);
        //Needed to wait for aiPath to realize it's no longer at end of path, or this method would get called again
        //TODO: find a fix for this. Easiest methord would be to force aiPath to recalculate, but its only available in the paid version
        yield return new WaitForSeconds(.5f);
        waitingForNewWanderTarget = false;
        aiPath.canMove = true;
    }

    //Stops the character from moving,
    private IEnumerator StopMovingAndPerformAttackAnimation()
    {
        currentlyAttacking = true;
        aiPath.canMove = false;
        yield return new WaitForSeconds(attackAnimationLength*.9f);
        aiPath.canMove = true;// same problem as mentioned on line 255, need to force aiPath to recalculate
        yield return new WaitForSeconds(attackAnimationLength * .1f);
        currentlyAttacking = false;

    }
    //Wait for the delay, and then deal the damage of the attack
    private IEnumerator DelayAndDealAttackDamage()
    {
        //wait
        yield return new WaitForSeconds(attackDamageDelay);
        Vector2 direction = GetDirectionFacing();
        //boxcast in the direction
        Vector2 attackOrigin = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + direction * attackBoxDistance;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(attackOrigin, attackBoxSize, 0, new Vector2(0, 0));

        foreach (RaycastHit2D hit in hits)
        {
            if ((hit.collider != null) ? hit.collider.gameObject.tag == "Player" : false)
            {
                EventManager.DamageActor(hit.collider.gameObject, gameObject, attackDamage, 0);
                Debug.Log(gameObject.name + " Hit Player!");
            }
        }
    }
    
    //wait for the delay and then reset cooldown
    private IEnumerator AttackCooldown()
    {
        attackOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        attackOnCooldown = false;
    }

    //Draw the attack square
    private void OnDrawGizmos()
    {
        if (!aiPath) return;
        //get Attack direction from 4 cardinal directions
        Vector2 direction = aiPath.desiredVelocity.normalized;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            direction = new Vector2(direction.x, 0);
            direction.Normalize();
        }
        else
        {
            direction = new Vector2(0, direction.y);
            direction.Normalize();
        }
        //boxcast in the direction
        Vector2 attackOrigin = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + direction * attackBoxDistance;
        //Draw Attack Cube
        Gizmos.DrawWireCube(new Vector3(attackOrigin.x, attackOrigin.y), new Vector3(attackBoxSize.x, attackBoxSize.y, 1));
    }
}
