using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseAttackComponent : MonoBehaviour
{
    [Header("Base Attack Settings")]
    [Tooltip("The damage to deal when the attack hits the player")]
    public int attackDamage = 1;
    [Tooltip("The time before the enemy will attack again")]
    public float attackCooldown = 1.2f;
    [Tooltip("The length of the attack animation")]
    public float attackAnimationLength = 1f;
    [Tooltip("Whether or not to cancel the attack when recieving damage")]
    public bool cancelAttackWhenDamaged = true;
    [Space(5)]


    protected bool currentlyAttacking = false;
    protected bool attackOnCooldown = false;
    protected EnemyAI aiComponent;
    protected Animator animator;
    protected GameObject playerGameObject;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        //Initialize PlayerGameObject
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        aiComponent = GetComponent<EnemyAI>();
        animator = GetComponent<Animator>();
        if(aiComponent == null)
        {
            Debug.LogError("No AI on object with attack, ObjectName: " + gameObject.name);
        }
        if (animator == null)
        {
            Debug.LogError("No animator on object with attack, ObjectName: " + gameObject.name);
        }
    }
    virtual public void TryPerformAttack()
    {
        if (!ShouldAttack()) return;
        if (currentlyAttacking || attackOnCooldown) return;
        StartCoroutine(AttackCooldown());
        StartAttackImplementation();    
    }

    virtual public bool GetIsAttacking()
    {
        return currentlyAttacking;
    }
    //wait for the delay and then reset cooldown
    protected virtual IEnumerator AttackCooldown()
    {
        attackOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        attackOnCooldown = false;
    }
    abstract protected void StartAttackImplementation();
    abstract protected bool ShouldAttack();
    abstract public void StopAttack();
}