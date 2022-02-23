using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public delegate void D_Void();
    public delegate void D_Bool(bool value);
    public delegate void D_Int(int value);
    public delegate void D_GameObject(GameObject _value);
    public delegate void D_GameObjecWithtInt(GameObject _value, int _value2);
    public delegate void D_TwoGameObjecWithtIntAndFloat(GameObject _value, GameObject _value1, int _value2, float _value3);
    public delegate void D_ItemWithInventoryWithInt(InventoryObject _inventory, ItemObject _item, int _amount);
    public delegate void D_ItemWithInventory(InventoryObject _inventory, ItemObject _item);
    public delegate void D_Inventory(InventoryObject _inventory);
    public delegate void D_ItemWithGameObject(ItemObject _item, GameObject _obj);
    public delegate void D_WeaponWithGameObject(WeaponObject _weapon, GameObject _obj);

    public static event D_GameObject onObjectDied;
    public static event D_Bool onControlsEnabled;
    public static event D_TwoGameObjecWithtIntAndFloat onDamageActor;
    public static event D_Inventory onInventoryChanged;
    public static event D_ItemWithGameObject onItemUse;
    public static event D_WeaponWithGameObject onWeaponEquip;

    public static void ControlsEnabled(bool value) { onControlsEnabled?.Invoke(value); }

    public static void ItemUse(ItemObject item, GameObject obj) { onItemUse?.Invoke(item, obj); }

    public static void ItemUse(ItemObject _item, GameObject _obj) { onItemUse?.Invoke(_item, _obj); }
    public static void WeaponEquip(WeaponObject _weapon, GameObject _obj) { onWeaponEquip?.Invoke(_weapon, _obj); }
    public static void InventoryChanged(InventoryObject _inventory) { onInventoryChanged?.Invoke(_inventory); }
    public static void DamageActor(GameObject _target, GameObject _attacker, int _dmg, float _knockback) { onDamageActor?.Invoke(_target, _attacker, _dmg, _knockback); }
    public static void ObjectDied(GameObject _obj) { onObjectDied?.Invoke(_obj); }
}