using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileAttackComponent : EnemyBaseAttackComponent
{
    [Header("Projectile Attack Settings")]
    [Tooltip("The projectile to spawn for the attack")]
    public GameObject projectilePrefab;
    [Tooltip("The speed to launch the projectile")]
    public float projectileSpeed = 3f;
    public float projectileSpawnDistance = .3f;
    [Tooltip("Width")]
    public float width = 1f;

    private Coroutine fireProjectileCoroutine;
    protected override bool ShouldAttack()
    {
        Vector2 playerLocation = playerGameObject.transform.position;
        Vector2 direction = playerGameObject.transform.position - gameObject.transform.position;

        return Mathf.Abs(direction.x) < width || Mathf.Abs(direction.y) < width;
    }

    protected override void StartAttackImplementation()
    {
        StartCoroutine(AnimateAndFireProjectile());
    }
    protected IEnumerator AnimateAndFireProjectile()
    {
        aiComponent.SetCanEnemyMove(false);
        animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(attackAnimationLength);

        animator.SetBool("isAttacking", false);
        aiComponent.SetCanEnemyMove(true);
        Vector3 SpawnLocation = transform.position;
        Vector3 direction = aiComponent.GetDirectionFacing();
        if(direction == Vector3.zero)direction = Vector3.down;
        SpawnLocation += direction * projectileSpawnDistance;
        GameObject projectile = Instantiate(projectilePrefab, SpawnLocation, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.attackDamage = this.attackDamage;
        Rigidbody2D projectileRigidBody = projectile.GetComponent<Rigidbody2D>();
        if (projectileRigidBody == null)
        {
            Debug.LogError("No RigidBody on Projectile");
        }
        else
        {
            projectileRigidBody.velocity = direction * projectileSpeed;
        }
    }
    public override void StopAttack()
    {
        if(fireProjectileCoroutine != null)StopCoroutine(fireProjectileCoroutine);
        animator.SetBool("isAttacking", false);
        aiComponent.SetCanEnemyMove(true);
    }
}
