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
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player.inventory.container.Count >= player.inventory.maxSize && !player.inventory.ItemExists(item.name))
            {
                EventManager.InventoryChanged(player.inventory);
            }
            else
            {
                anim.SetTrigger("Open");
                player.inventory.AddItem(item, amount);
            }
        }
    }
}
