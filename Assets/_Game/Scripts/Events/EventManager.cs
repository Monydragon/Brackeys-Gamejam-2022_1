using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public delegate void D_Void();
    public delegate void D_Int(int value);
    public delegate void D_GameObjectInt(GameObject value, int value2);
    public delegate void D_ItemWithInventoryWithInt(InventoryObject inventory, ItemObject item, int amount);
    public delegate void D_ItemWithInventory(InventoryObject inventory, ItemObject item);
    public delegate void D_Inventory(InventoryObject inventory);
    public delegate void D_ItemWithGameObject(ItemObject item, GameObject obj);


    public static event D_Inventory onInventoryChanged;
    public static event D_ItemWithGameObject onItemUse;


    public static void ItemUse(ItemObject item, GameObject obj) { onItemUse?.Invoke(item, obj); }

    public static void InventoryChanged(InventoryObject inventory) { onInventoryChanged?.Invoke(inventory); }
}