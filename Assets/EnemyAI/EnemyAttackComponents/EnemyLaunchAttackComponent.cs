using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaunchAttackComponent : EnemyBaseAttackComponent
{
    [Header("Launch Attack Settings")]
    [Tooltip("The time to be launched before stopping")]
    public float launchTime = 1f;
    [Tooltip("The time to be launched  backwards before launching forwards")]
    public float windUpTime = .2f;
    [Tooltip("The force to launch with")]
    public float launchForce = 1000f;
    [Tooltip("The force to launch with")]
    public float windUpForce = 200f;
    [Tooltip("Width")]
    public float width = 1f;
    public bool StartAnimationDuringWindUp = false;
    [Tooltip("ObstacleLayer")]
    public LayerMask ObstacleLayer;

    private Collider2D bodyCollider;
    private Rigidbody2D rb;
    private Coroutine launchCoroutine;
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        bodyCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        if(rb == null)
        {
            Debug.LogError("Object with Launch Attack has no rigidbody. ObjectName: " + gameObject.name);
        }
    }
    protected override void StartAttackImplementation()
    {
        launchCoroutine = StartCoroutine(LaunchHitAndStop());
    }
    protected override bool ShouldAttack()
    {
        Vector2 playerLocation = playerGameObject.transform.position;
        Vector2 direction = playerGameObject.transform.position-gameObject.transform.position;

        return Mathf.Abs(direction.x) < width || Mathf.Abs(direction.y) < width;
    }
    private IEnumerator LaunchHitAndStop()
    {
        if(StartAnimationDuringWindUp) animator.SetBool("isAttacking", true);
        //Wind up
        rb.velocity = Vector2.zero;
        aiComponent.SetCanEnemyMove(false);
        Vector2 direction = aiComponent.GetDirectionFacing();
        if (direction == Vector2.zero) { direction = Vector2.down; }
        rb.AddForce(direction * -windUpForce);

        //wait for wind up
        yield return new WaitForSeconds(windUpTime);

        //start attack launch
        rb.velocity = Vector2.zero;
        currentlyAttacking = true;
        bodyCollider.isTrigger = true;
        rb.AddForce(direction * launchForce);
        if (!StartAnimationDuringWindUp) animator.SetBool("isAttacking", true);

        // wait for launch
        yield return new WaitForSeconds(launchTime);

        //go back to moving normal
        bodyCollider.isTrigger = false;
        aiComponent.SetCanEnemyMove(true);
        animator.SetBool("isAttacking", false);
        currentlyAttacking = false;
    }

    public override void StopAttack()
    {
        if(launchCoroutine !=null)StopCoroutine(launchCoroutine);
        
        //go back to moving normal
        bodyCollider.isTrigger = false;
        aiComponent.SetCanEnemyMove(true);
        animator.SetBool("isAttacking", false);
        currentlyAttacking = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentlyAttacking)
        {
            if(collision.gameObject.tag == "Player")
            {
                Debug.Log(gameObject.name + " Hit Player!");
                EventManager.DamageActor(collision.gameObject, gameObject, attackDamage, 0);
            }
            else if ((ObstacleLayer.value & 1 << collision.gameObject.layer) > 0)
            {
                StopAttack();
            }
        }
    }
}
