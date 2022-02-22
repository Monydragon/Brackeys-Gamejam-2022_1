using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InventoryObject inventory;
    [Header("Combat Status")]
    public WeaponObject weapon;
    [Tooltip("the bigger the number, more slower the cooldown!")]
    [Range(0.1f, 5f)]
    public float attackSpeed;
    [Range(1, 10)]
    public int strength;
    [SerializeField]
    private Animator playerAnim;

    // movement variable
    [Header("Movement costumizesation")]
    [Tooltip("allow player to move or not, usefull for cutscene")]
    public bool MovementEnable;
    [Tooltip("maximum speed when normaly walking")]
    [Range(0, 100)]
    public float MaxWalkingSpeed;
    [Tooltip("maximum speed when running")]
    [Range(0, 100)]
    public float MaxRunSpeed;
    [Tooltip("time needed before getting to full speed(in second)")]
    [Range(0.1f, 1f)]
    public float accelerationTime;
    [Tooltip("time needed before the player stoped(in second)")]
    [Range(0.1f, 1f)]
    public float deaccelerationTime;
    [Tooltip("maximum time delay between the first move button press and the second one to triger running")]
    [Range(0.1f, 1)]
    public float doubleTapForRunningCooldown;

    // private movement variable
    private Vector2 input;
    private Vector2 acceleration;
    private Vector2 accelerationMult;
    private Vector2 movement = Vector2.zero;
    private Vector2 directionFacing = Vector2.up;
    private bool[] moveBefore = new bool[2];
    private float MaxSpeed;
    [HideInInspector]
    public bool running = false;
    private Vector2 RunningTimer = Vector2.zero;
    private Vector2 RunningDirections = Vector2.zero;
    private Vector2 lastInput = Vector2.zero;
    private Vector2 lastMove;


    [HideInInspector]
    public Rigidbody2D body;
    public HealthSystem health;

    private void OnEnable()
    {
        EventManager.onItemUse += EventManager_onItemUse;
    }

    private void EventManager_onItemUse(ItemObject _item, GameObject _obj)
    {
        if (_obj == this.gameObject)
        {
            Debug.Log($"Player Used Item: {_item.name}");
            inventory.RemoveItem(_item);
        }
    }

    private void OnDisable()
    {
        EventManager.onItemUse -= EventManager_onItemUse;
    }

    // Start is called before the first frame update
    void Start()
    {
        //setup all the references variable
        body = GetComponent<Rigidbody2D>();
        health = GetComponent<HealthSystem>();

        EventManager.DamageActor(this.gameObject, 5);

    }

    // Update is called once per frame
    void Update()
    {
        // getting input for movement
        if (MovementEnable)
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // check if running
        if (lastInput != input && input != Vector2.zero)
        {
            if (RunningDirections == input)
                running = true;
            else
                for (int i = 0; i < 2; i++)
                    RunningTimer[i] = doubleTapForRunningCooldown;
            RunningDirections = input;
        }

        for (int i = 0; i < 2; i++)
        {
            if (RunningTimer[i] <= 0)
            {
                RunningTimer[i] = 0;
                RunningDirections[i] = 0;
            }
            else
                RunningTimer[i] -= Time.deltaTime;
        }

        if (input == Vector2.zero)
            running = false;

        lastInput = input;

        if (Input.GetKeyDown(KeyCode.L))
            inventory.Load();
        if (Input.GetKeyDown(KeyCode.J))
            inventory.Save();

        // Combat System
        GameObject Hand;
        Hand = transform.GetChild(0).gameObject;
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Hand.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        if (cooldownTimer <= 0)
        {
            attackBefore = false;
            cooldownTimer = 0;
        }
        else
            cooldownTimer -= Time.deltaTime;
        if (Input.GetAxisRaw("Fire1") == 1 && !attackBefore)
        {
            attackBefore = true;
            cooldownTimer = weapon.coolDown * attackSpeed;
            foreach (GameObject enemy in EnemyInRange)
            {
                EventManager.DamageActor(enemy, (int)(weapon.damage * strength));
            }
        }
    }
    bool attackBefore = false;
    float cooldownTimer;
    public string[] killebleObject = { "Enemy" };
    public List<GameObject> EnemyInRange = new List<GameObject>();
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.isInside(killebleObject))
            EnemyInRange.Add(collision.gameObject);
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.isInside(killebleObject))
            EnemyInRange.Remove(collision.gameObject);
    }
    void FixedUpdate()
    {
        // movement script, no need to understand it :I, but if you need dm me maybe? @Shaladdin
        MaxSpeed = running ? MaxRunSpeed : MaxWalkingSpeed;
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
                movement[i] = input[i] * MaxSpeed * accelerationMult[i];
                directionFacing[i] = input[i];
            }
            else
                movement[i] = MaxSpeed * accelerationMult[i] * directionFacing[i];

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

        body.velocity = movement;
    }
    private void OnApplicationQuit()
    {
        inventory.container.Clear();
        inventory.currentSize = 0;
    }
}
