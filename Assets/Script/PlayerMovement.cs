using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // variable that shows in the inspector :] (yes i use these emoji all the time)

    [Header("public variable")]
    [Tooltip("allow player to move or not, usefull for cutscene")]
    public bool MovementEnable;
    [Header("Movement costumizesation")]
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


    [Header("Debuging purpose, Read only please :D")]
    [SerializeField]
    private Vector2 input;
    [SerializeField]
    private Vector2 acceleration;
    [SerializeField]
    private Vector2 accelerationMult;
    [SerializeField]
    private Vector2 movement = Vector2.zero;
    [SerializeField]
    private Vector2 directionFacing = Vector2.up;
    [SerializeField]
    private bool[] moveBefore = new bool[2];
    [SerializeField]
    private float MaxSpeed;
    public bool running = false;

    [SerializeField]
    private Vector2 RunningTimer = Vector2.zero;
    [SerializeField]
    private Vector2 RunningDirections = Vector2.zero;
    [SerializeField]
    private Vector2 lastInput = Vector2.zero;

    [HideInInspector]
    public Rigidbody2D body;

    void Awake()
    {
        //setup all the references variable
        body = GetComponent<Rigidbody2D>();
    }

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

    }

    void FixedUpdate()
    {
        // movement script, no need to understand it :I, but if you need dm me maybe? @Shaladdin
        MaxSpeed = running ? MaxRunSpeed : MaxWalkingSpeed;
        // TODO: currently it used 2 if statement, one for the x, and the other for y. i want to make it into single line so my ocd didnt actout again
        // nevermind, unity's pointer is seems more complicated than c++ ponter :L
        // nevermind, i can just use loops and use vector2 like array
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
        body.velocity = movement;
    }
}

// TODO:
/*
FEATURE:

BUGS:
1. if the player change movement from moving left then instanly to the right, it will instantly change direction hardly
2. the stop time must slower than the acceleration time, need to fix it later :I
*/