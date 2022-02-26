using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public bool flashColorOnHit;
    public Color flashColor;
    public int flashAmount;
    public float healthDamageFlashSpeed;
    public float destroyTime = 1.5f;
    [HideInInspector]
    public Rigidbody2D body;
    public float dieTimer = 1;
    private bool dying = false;
    private SpriteRenderer spriterender;

    // subcribe to ondamage
    private void OnEnable()
    {
        EventManager.onDamageActor += TakeDamage;
        EventManager.onObjectDied += Die;
        EventManager.onHealthAdd += HealthAdd;
        if (gameObject.tag == "Player")
        {
            EventManager.PlayerHealthChanged(health, maxHealth);
        }
    }

    private void HealthAdd(GameObject _value, int _healthToAdd)
    {
        if(gameObject == _value)
        {
            health = Mathf.Min(health + _healthToAdd, maxHealth);
            if(gameObject.tag == "Player")
            {
                EventManager.PlayerHealthChanged(health, maxHealth);
            }
        }
    }

    // unsubcribe 
    private void OnDisable()
    {
        EventManager.onDamageActor -= TakeDamage;
        EventManager.onObjectDied -= Die;
        EventManager.onHealthAdd -= HealthAdd;
    }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spriterender = GetComponent<SpriteRenderer>();
    }

    void TakeDamage(GameObject _target, GameObject _attacker, int _damage, float _knockback)
    {
        if (_target == this.gameObject && !dying)
        {
            StartCoroutine(FlashDamage());
            health -= _damage;
            // knockback
            Vector3 diff = transform.position - _attacker.transform.position;
            body.velocity += (new Vector2(diff.x, diff.y).normalized * _knockback);

            // Send Health Update message if this is the player
            if(gameObject.tag == "Player")
            {
                EventManager.PlayerHealthChanged(health, maxHealth);
            }

            // if die
            if (health <= 0)
                // call an event for loot drop or something
                EventManager.ObjectDied(gameObject);
        }
    }

    void Die(GameObject _obj)
    {
        if (_obj == this.gameObject && !dying)
        {
            dying = true;
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

    void Update()
    {

    }

    IEnumerator FlashDamage()
    {
        if (flashColorOnHit)
        {
            if (spriterender != null)
            {
                for (int i = 0; i < flashAmount; i++)
                {
                    spriterender.color = flashColor;
                    yield return new WaitForSeconds(healthDamageFlashSpeed);
                    spriterender.color = Color.white;
                    yield return new WaitForSeconds(healthDamageFlashSpeed);
                }

            }
        }
        
    }
}
