using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartContainer : MonoBehaviour
{
    public int amountToIncreaseMaxHealth = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            HealthComponent healthComponent = collision.gameObject.GetComponent<HealthComponent>();
            if (healthComponent == null)
            {
                Debug.LogError("Player Has No Health Component");
                Destroy(gameObject);
                return;
            }
            healthComponent.MaxHealthAdd(amountToIncreaseMaxHealth);
            Destroy(gameObject);
        }
    }
}
