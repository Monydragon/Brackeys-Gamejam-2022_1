using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    // Player Pref Keys
    public static readonly string PLAYER_HEALTH = "playerHealth";
    public static readonly string PLAYER_MAX_HEALTH = "playerMaxHealth";

    public int health;
    public int maxHealth;
    public bool flashColorOnHit;
    public Color flashColor;
    public int flashAmount;
    public float healthDamageFlashSpeed;
    public float destroyTime = 1.5f;
    public AudioClip damagedAudioClip;
    public AudioClip healAudioClip;
    [HideInInspector]
    public Rigidbody2D body;
    public float dieTimer = 1;
    private bool dying = false;
    private SpriteRenderer spriterender;
    private AudioSource audioSource;
    // subcribe to ondamage
    private void OnEnable()
    {
        EventManager.onDamageActor += TakeDamage;
        EventManager.onObjectDied += Die;
        EventManager.onHealthAdd += HealthAdd;
        EventManager.onHeartContainerIncrease += MaxHealthAdd;
        EventManager.onSavePlayerInventory += SaveHealth;
        if (gameObject.tag == "Player")
        {
            health = PlayerPrefs.GetInt(PLAYER_HEALTH);
            maxHealth = PlayerPrefs.GetInt(PLAYER_MAX_HEALTH);
            EventManager.PlayerHealthChanged(health, maxHealth);
        }
    }

    private void MaxHealthAdd(HeartContainerObject _container, GameObject _obj)
    {
        if(_obj == gameObject)
        {
            maxHealth += _container.healthIncreaseAmount;
            health = maxHealth;
            if(gameObject.tag == "Player")
            {
                EventManager.PlayerHealthChanged(health, maxHealth);
            }
        }
    }
    public void MaxHealthAdd(int _amountToAdd)
    {
        maxHealth += _amountToAdd;
        health = maxHealth;
        if(gameObject.tag == "Player")
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
                audioSource.PlayOneShot(healAudioClip);
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
        EventManager.onHeartContainerIncrease -= MaxHealthAdd;
        EventManager.onSavePlayerInventory -= SaveHealth;
    }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spriterender = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void TakeDamage(GameObject _target, GameObject _attacker, int _damage, float _knockback)
    {
        if (_target == this.gameObject && !dying)
        {
            StartCoroutine(FlashDamage());
            audioSource.PlayOneShot(damagedAudioClip);
            health = Mathf.Max(0, health - _damage);
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
                    StartCoroutine(GameSystems.Instance.StateManager.NavigateToState(typeof(GameOverState)));
                    EventManager.ControlsEnabled(false);
                }
            }

            if(gameObject.tag != "Player")
            {
                // destroy gameObject
                Destroy(gameObject, destroyTime);
            }
        }
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

    public static void ResetPlayerHealth()
    {
        PlayerPrefs.SetInt(PLAYER_HEALTH, 10);
        PlayerPrefs.SetInt(PLAYER_MAX_HEALTH, 10);
        PlayerPrefs.Save();
    }

    private void SaveHealth()
    {
        if (gameObject.tag == "Player")
        {
            PlayerPrefs.SetInt(PLAYER_HEALTH, health);
            PlayerPrefs.SetInt(PLAYER_MAX_HEALTH, maxHealth);
            PlayerPrefs.Save();
        }
    }
}
