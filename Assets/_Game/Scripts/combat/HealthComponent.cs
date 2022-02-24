using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int health;
    public float destroyTime = 1.5f;
    [HideInInspector]
    public Rigidbody2D body;

    // subcribe to ondamage
    private void OnEnable()
    {
        EventManager.onDamageActor += TakeDamage;
        EventManager.onObjectDied += Die;
    }
    // unsubcribe 
    private void OnDisable()
    {
        EventManager.onDamageActor -= TakeDamage;
        EventManager.onObjectDied -= Die;
    }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void TakeDamage(GameObject _target, GameObject _attacker, int _damage, float _knockback)
    {
        if (_target == this.gameObject)
        {
            health -= _damage;
            // knockback
            Vector3 diff = transform.position - _attacker.transform.position;
            body.velocity += (new Vector2(diff.x, diff.y).normalized * _knockback);
            // if die
            if (health <= 0)
                // call an event for loot drop or something
                EventManager.ObjectDied(gameObject);
        }
    }

    void Die(GameObject _obj)
    {
        if (_obj == this.gameObject)
        {
            var anim = _obj.GetComponent<Animator>();
            if(anim != null)
            {
                anim.SetTrigger("isDead");
                if(_obj.tag == "Player")
                {
                    EventManager.ControlsEnabled(false);
                }
            }
            // destroy gameObject
            Destroy(gameObject, destroyTime);
        }
    }

}
