using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int attackDamage { get; set; }

    private Collider2D projectileCollider;

    [Tooltip("ObstacleLayer")]
    public LayerMask ObstacleLayer;
    // Start is called before the first frame update
    void Start()
    {
        projectileCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(gameObject.name + " Hit Player!");
            EventManager.DamageActor(collision.gameObject, gameObject, attackDamage, 0);
            Destroy(gameObject);
        }
        else if ((ObstacleLayer.value & 1 << collision.gameObject.layer) > 0)
        {
            Destroy(gameObject);
        } 
    }
}
