using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemObject item;
    public int amount = 1;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = item.icon;
        gameObject.name = item.name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.tag == "Player")
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if(player.inventory.currentSize >= player.inventory.maxSize && !player.inventory.ItemExists(item.name))
            {
                EventManager.InventoryChanged(player.inventory);
            }
            else
            {
                player.inventory.AddItem(item, amount);
                Destroy(gameObject);
            }
        }
    }
}
