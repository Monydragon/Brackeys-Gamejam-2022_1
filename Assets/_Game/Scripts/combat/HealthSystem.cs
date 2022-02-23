using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int health;
    [HideInInspector]
    public Rigidbody2D body;

    // subcribe to ondamage
    private void OnEnable()
    {
        EventManager.onDamageActor += takeDamage;
    }
    // unsubcribe 
    private void OnDisable()
    {
        EventManager.onDamageActor -= takeDamage;
    }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void takeDamage(GameObject target, GameObject attacker, int damage, float knockback)
    {
        if (target == this.gameObject)
        {
            health -= damage;
            // knockback
            Vector3 diff = transform.position - attacker.transform.position;
            body.velocity += (new Vector2(diff.x, diff.y).normalized * knockback) +
            attacker.GetComponent<Rigidbody2D>().velocity;
            // if die
            if (health <= 0)
            {
                // call an event for loot drop or something
                EventManager.ObjectDied(gameObject);
                // destroy gameObject
                Destroy(gameObject);
            }
        }
    }


}
