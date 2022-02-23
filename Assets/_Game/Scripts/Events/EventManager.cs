using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public delegate void D_Void();
    public delegate void D_Bool(bool value);
    public delegate void D_Int(int value);
    public delegate void D_GameObject(GameObject value);
    public delegate void D_TwoGameObjecWithtIntAndFloat(GameObject value, GameObject value1, int value2, float value3);
    public delegate void D_ItemWithInventoryWithInt(InventoryObject inventory, ItemObject item, int amount);
    public delegate void D_ItemWithInventory(InventoryObject inventory, ItemObject item);
    public delegate void D_GameObjecWithtInt(GameObject value, int value2);
    public delegate void D_Inventory(InventoryObject inventory);
    public delegate void D_ItemWithGameObject(ItemObject item, GameObject obj);


    public static event D_GameObject onObjectDied;
    public static event D_Bool onControlsEnabled;
    public static event D_TwoGameObjecWithtIntAndFloat onDamageActor;
    public static event D_Inventory onInventoryChanged;
    public static event D_ItemWithGameObject onItemUse;

    public static void ControlsEnabled(bool value) { onControlsEnabled?.Invoke(value); }


    public static void ItemUse(ItemObject item, GameObject obj) { onItemUse?.Invoke(item, obj); }

    public static void InventoryChanged(InventoryObject inventory) { onInventoryChanged?.Invoke(inventory); }
    public static void DamageActor(GameObject target, GameObject attacker, int dmg, float knockback) { onDamageActor?.Invoke(target, attacker, dmg, knockback); }
    public static void ObjectDied(GameObject obj) { onObjectDied?.Invoke(obj); }
}