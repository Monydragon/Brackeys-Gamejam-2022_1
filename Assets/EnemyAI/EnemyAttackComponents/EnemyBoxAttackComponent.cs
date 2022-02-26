using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoxAttackComponent : EnemyBaseAttackComponent
{
    [Header("Box Attack Settings")]
    [Tooltip("The distance to spawn the hit box for the enemies attack")]
    public float attackBoxDistance = 1f;
    [Tooltip("The size of the hitbox")]
    public Vector2 attackBoxSize = new Vector2(1, 1);
    [Tooltip("The time to delay the damage of the attack once it has started")]
    public float attackDamageDelay = .5f;
    [Tooltip("The minimum range to the player, before the enemy will try to perform an attack")]
    public float rangeToAttackPlayer = 1.3f;

    private Coroutine AttackAnimCoroutine;
    private Coroutine AttackDamageCoroutine;
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
    }
    protected override void StartAttackImplementation()
    {
        AttackAnimCoroutine = StartCoroutine(StopMovingAndPerformAttackAnimation());
        AttackDamageCoroutine = StartCoroutine(DelayAndDealAttackDamage());
    }

    protected override bool ShouldAttack()
    {
        float distance = (playerGameObject.transform.position - gameObject.transform.position).magnitude;
        return distance < rangeToAttackPlayer;
    }

    //Stops the character from moving,
    private IEnumerator StopMovingAndPerformAttackAnimation()
    {
        currentlyAttacking = true;
        aiComponent.SetCanEnemyMove(false);
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(attackAnimationLength * .9f);
        aiComponent.SetCanEnemyMove(true);// need to give aiPath to recalculate
        yield return new WaitForSeconds(attackAnimationLength * .1f);
        animator.SetBool("isAttacking", false);
        currentlyAttacking = false;

    }
    //Wait for the delay, and then deal the damage of the attack
    private IEnumerator DelayAndDealAttackDamage()
    {
        //wait
        yield return new WaitForSeconds(attackDamageDelay);
        Vector2 direction = aiComponent.GetDirectionFacing();
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
    //Draw the attack square
    private void OnDrawGizmos()
    {
        if (!aiComponent) return;
        //get Attack direction from 4 cardinal directions
        Vector2 direction = aiComponent.GetDirectionFacing();
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

    public override void StopAttack()
    {

        if(AttackAnimCoroutine != null)StopCoroutine(AttackAnimCoroutine);
        if(AttackDamageCoroutine != null)StopCoroutine(AttackDamageCoroutine);
        
        aiComponent.SetCanEnemyMove(true);
        animator.SetBool("isAttacking", false);
        currentlyAttacking = false;
    }
}
