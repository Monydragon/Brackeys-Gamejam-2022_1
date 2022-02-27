using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    public int minimumItemDrop;
    public int maximumItemDrop;
    public Loot[] loot;
    private Animator anim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.tag == "Player")
        {
            int dropedItem = Random.Range(minimumItemDrop, maximumItemDrop + 1);
            anim = GetComponent<Animator>();
            var player = collision.gameObject.GetComponent<PlayerController>();

            for (int i = 0; i < dropedItem; i++)
            {
                foreach (Loot _loot in loot)
                {
                    if (_loot.alwaysDrop || Random.Range(0, _loot.rarenes + 1) == 0)
                    {
                        int amount = Random.Range(_loot.minimumDrop, _loot.maximumDrop + 1);
                        if (amount > dropedItem)
                        {
                            amount = dropedItem;
                        }
                        if (amount > 0 && !_loot.alreadyDroped)
                        {
                            _loot.alreadyDroped = true;
                            for (int j = 0; j < amount; j++)
                            {
                                if (player.inventory.container.Count >= player.inventory.maxSize && !player.inventory.ItemExists(_loot.item.name))
                                {
                                    EventManager.InventoryChanged(player.inventory);
                                    continue;
                                }
                                else
                                {
                                    anim.SetTrigger("Open");
                                    player.inventory.AddItem(_loot.item, amount);
                                }

                            }
                                
                        }
                        i += amount;
                    }
                    if (!(i < dropedItem))
                        break;
                }
            }
        }
    }
}
