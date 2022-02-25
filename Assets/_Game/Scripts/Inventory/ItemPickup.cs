using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemObject item;
    public int amount = 1;

    private void Awake()
    {
        if (item != null)
            Setup();
    }
    private void Setup()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.icon;
        spriteRenderer.sortingLayerName = "Object";
        gameObject.name = item.name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.tag == "Player")
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player.inventory.currentSize >= player.inventory.maxSize && !player.inventory.ItemExists(item.name))
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

    public static GameObject newItem(ItemObject _item, int _amount, Vector3 _position)
    {
        GameObject obj = new GameObject(_item.name, typeof(SpriteRenderer));
        ItemPickup objItem = obj.AddComponent<ItemPickup>();
        BoxCollider2D objTriger = obj.AddComponent<BoxCollider2D>();

        obj.transform.position = _position;

        // setup the item
        objItem.item = _item;
        objItem.amount = _amount;

        objTriger.isTrigger = true;
        objTriger.size = Vector2.one;

        objItem.Setup();
        return obj;
    }
}
