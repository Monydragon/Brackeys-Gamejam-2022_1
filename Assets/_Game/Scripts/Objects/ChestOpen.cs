using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    public ItemObject item;
    public int amount;
    private Animator anim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.tag == "Player")
        {
            anim = GetComponent<Animator>();
            anim.SetTrigger("Open");
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player.inventory.currentSize >= player.inventory.maxSize && !player.inventory.ItemExists(item.name))
            {
                EventManager.InventoryChanged(player.inventory);
            }
            else
            {
                player.inventory.AddItem(item, amount);
            }
        }
    }
}
