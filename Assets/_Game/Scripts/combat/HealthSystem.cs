using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int health;

    private void OnEnable()
    {
        EventManager.onDamageActor += takeDamage;
    }

    private void OnDisable()
    {
        EventManager.onDamageActor -= takeDamage;
    }

    void takeDamage(GameObject obj, int damage)
    {
        if (obj == this.gameObject)
            health -= damage;
        // if die
        if (health <= 0)
        {
            // destroy gameObject
            Destroy(gameObject);
        }
    }


}
