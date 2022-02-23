using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "DLS/Weapon")]
public class WeaponObject : ItemObject
{
    [Range(1, 10)]
    public int damage;
    [Range(0f, 5f)]
    public float coolDown;
    [Range(0f, 10f)]
    public float knockback;
    [Tooltip("in second")]
    [Range(0f, 1f)]
    public float attackDuration;

    override public void Use(GameObject obj)
    {
        EventManager.WeaponEquip(this, obj);
    }

}