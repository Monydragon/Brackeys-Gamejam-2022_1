using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlotUse : MonoBehaviour
{
    public ItemObject item;

    public void UseItem()
    {
        if(item != null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            item.Use(player);
        }
        else
        {
            Debug.LogWarning($"Item is NULL");
        }
    }
}
