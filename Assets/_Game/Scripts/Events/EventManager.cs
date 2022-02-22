using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public delegate void D_Void();
    public delegate void D_Bool(bool value);
    public delegate void D_Inventory(InventoryObject inventory);
    public delegate void D_ItemWithGameObject(ItemObject item, GameObject obj);

    public delegate void D_GameObjectWithInt(GameObject obj, int dmg);


    public static event D_GameObjectWithInt onDamageActor;
    public static event D_Bool onControlsEnabled;
    public static event D_Inventory onInventoryChanged;
    public static event D_ItemWithGameObject onItemUse;

    public static void ControlsEnabled(bool value) { onControlsEnabled?.Invoke(value); }


    public static void ItemUse(ItemObject item, GameObject obj) { onItemUse?.Invoke(item, obj); }

    public static void InventoryChanged(InventoryObject inventory) { onInventoryChanged?.Invoke(inventory); }
    public static void DamageActor(GameObject obj, int dmg) { onDamageActor?.Invoke(obj, dmg); }
}