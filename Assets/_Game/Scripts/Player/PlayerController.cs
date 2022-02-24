using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InventoryObject inventory;
    [Header("Combat")]
    public WeaponObject weapon;
    public CombatStats stats;
    public string[] killebleObject = { "Enemy" };
    // movement variable
    [Header("Movement customization")]
    [Tooltip("allow player to move or not, usefull for cutscene")]
    public bool movementEnable;
    [Tooltip("maximum speed when normaly walking")]
    [Range(0, 100)]
    public float maxWalkingSpeed;
    [Tooltip("maximum speed when running")]
    [Range(0, 100)]
    public float maxRunSpeed;
    [Tooltip("time needed before getting to full speed(in second)")]
    [Range(0.1f, 1f)]
    public float accelerationTime;
    [Tooltip("time needed before the player stoped(in second)")]
    [Range(0f, 1f)]
    public float deaccelerationTime;
    [Tooltip("maximum time delay between the first move button press and the second one to triger running")]
    [Range(0.1f, 1)]
    public float doubleTapForRunningCooldown;


    [HideInInspector]
    public Rigidbody2D body;
    [HideInInspector]
    public HealthComponent health;

    [Header("Combat debugging purpose")]
    public List<GameObject> enemyInRange = new List<GameObject>();

    [SerializeField]
    private Animator playerAnim;
    // private movement variable
    private Vector2 input;
    private Vector2 acceleration;
    private Vector2 accelerationMult;
    private Vector2 movement = Vector2.zero;
    private Vector2 directionFacing = Vector2.up;
    private bool[] moveBefore = new bool[2];
    private float maxSpeed;
    [HideInInspector]
    public bool running = false;
    private Vector2 runningTimer = Vector2.zero;
    private Vector2 runningDirections = Vector2.zero;
    private Vector2 lastInput = Vector2.zero;
    private Vector2 lastMove;



    // combat variable
    private bool currentlyAttacking = false;



    // subcribe to event manager and stuff
    private void OnEnable()
    {
        EventManager.onItemUse += EventManager_onItemUse;
        EventManager.onWeaponEquip += EventManager_onWeaponEquip;
        EventManager.onControlsEnabled += EventManager_onControlsEnabled;
        EventManager.onObjectDied += EventManager_onObjectDied;
    }

    private void EventManager_onObjectDied(GameObject _value)
    {
        if (enemyInRange.Contains(_value))
        {
            enemyInRange.Remove(_value);
        }
    }

    private void EventManager_onControlsEnabled(bool value)
    {
        movementEnable = value;
    }

    private void EventManager_onItemUse(ItemObject _item, GameObject _obj)
    {
        if (_obj == this.gameObject)
        {
            Debug.Log($"Player Used Item: {_item.name}");
            inventory.RemoveItem(_item);
        }
    }
    private void EventManager_onWeaponEquip(WeaponObject _weapon, GameObject _obj)
    {
        if (_obj == this.gameObject)
        {
            Debug.Log($"Player Equip Item: {_weapon.name}");
            inventory.RemoveItem(_weapon);
            weapon = _weapon;
        }
    }
    private void OnDisable()
    {
        EventManager.onItemUse -= EventManager_onItemUse;
        EventManager.onWeaponEquip -= EventManager_onWeaponEquip;
        EventManager.onControlsEnabled -= EventManager_onControlsEnabled;
        EventManager.onObjectDied -= EventManager_onObjectDied;
    }


    // Start is called before the first frame update
    void Start()
    {
        //setup all the references variable
        body = GetComponent<Rigidbody2D>();
        health = GetComponent<HealthComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movementEnable)
        {
            GetInput();
            Movement();
        }
        else
            // just making the player idling
            playerAnim.SetBool("isMoving", false);
        Combat();
        SaveLoadSystem();
    }


    // for Combat stuff, 
    // basicly checking if the enemy is in range
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.isInside(killebleObject))
        {
            enemyInRange.Add(collision.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.isInside(killebleObject))
            enemyInRange.Remove(collision.gameObject);
    }


    private void OnApplicationQuit()
    {
        inventory.container.Clear();
        inventory.currentSize = 0;
    }




    // some update code, orgenized :D
    private void SaveLoadSystem()
    {
        if (Input.GetKeyDown(KeyCode.L))
            inventory.Load();
        if (Input.GetKeyDown(KeyCode.J))
            inventory.Save();
    }
    private void GetInput()
    {
        // getting input for movement
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // check if running
        if (lastInput != input && input != Vector2.zero)
        {
            if (runningDirections == input)
                running = true;
            else
                for (int i = 0; i < 2; i++)
                    runningTimer[i] = doubleTapForRunningCooldown;
            runningDirections = input;
        }

        for (int i = 0; i < 2; i++)
        {
            if (runningTimer[i] <= 0)
            {
                runningTimer[i] = 0;
                runningDirections[i] = 0;
            }
            else
                runningTimer[i] -= Time.deltaTime;
        }

        if (input == Vector2.zero)
            running = false;

        lastInput = input;
    }
    private void Movement()
    {
        // movement script, no need to understand it :I, but if you need dm me maybe? @Shaladdin
        maxSpeed = running ? maxRunSpeed : maxWalkingSpeed;
        for (int i = 0; i < 2; i++)
        {
            if (input[i] != 0)
            {
                acceleration[i] += Time.fixedDeltaTime;
                if (acceleration[i] > accelerationTime)
                    acceleration[i] = accelerationTime;
                moveBefore[i] = true;
            }
            else
            {
                if (moveBefore[i])
                {
                    moveBefore[i] = false;
                    acceleration[i] = deaccelerationTime;
                }
                else
                    acceleration[i] -= Time.fixedDeltaTime;
                if (acceleration[i] < 0)
                    acceleration[i] = 0;
            }
        }

        accelerationMult = common.map(acceleration, 0, accelerationTime, 0, 1);

        for (int i = 0; i < 2; i++)
        {
            if (input[i] != 0)
            {
                movement[i] = input[i] * maxSpeed * accelerationMult[i];
                directionFacing[i] = input[i];
            }
            else
                movement[i] = maxSpeed * accelerationMult[i] * directionFacing[i];
        }

        // animation manager

        Vector2 newInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        playerAnim.SetFloat("MoveX", newInput.x);
        playerAnim.SetFloat("MoveY", newInput.y);
        if (input != Vector2.zero)
        {
            playerAnim.SetBool("isMoving", true);
            lastMove = newInput;
            playerAnim.SetFloat("LastMoveX", lastMove.x);
            playerAnim.SetFloat("LastMoveY", lastMove.y);
        }
        else
            playerAnim.SetBool("isMoving", false);

        body.MovePosition(new Vector2(transform.position.x, transform.position.y) + movement * Time.deltaTime);
        body.velocity = Vector2.zero;
    }
    private void Combat()
    {
        // Combat System
        GameObject Hand;
        Hand = transform.GetChild(0).gameObject;
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Hand.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        // if attack
        if (Input.GetAxisRaw("Fire1") == 1 && !currentlyAttacking && weapon != null)
        {
            StartCoroutine(WaitForAttackCoolDown());

            foreach (GameObject enemy in enemyInRange.ToList())
            {
                if(enemy != null)
                {
                    EventManager.DamageActor(enemy, gameObject,
                    (int)(weapon.damage * stats.strength), stats.knockback * weapon.knockback);

                    Vector3 diffrence = enemy.transform.position - transform.position;
                    lastMove = new Vector2(diffrence.x, diffrence.y).normalized;
                    playerAnim.SetFloat("LastMoveX", lastMove.x);
                    playerAnim.SetFloat("LastMoveY", lastMove.y);
                }
            }
        }
    }

    private IEnumerator WaitForAttackCoolDown()
    {
        var coolDownTimer = weapon.coolDown * stats.attackSpeed;
        currentlyAttacking = true;
        playerAnim.SetBool("isAttacking", true);
        yield return new WaitForSeconds(coolDownTimer);
        currentlyAttacking = false;
        playerAnim.SetBool("isAttacking", false);
    }
}