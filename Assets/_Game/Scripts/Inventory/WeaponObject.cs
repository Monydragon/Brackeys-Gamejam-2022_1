using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "DLS/Weapon")]
public class WeaponObject : ItemObject
{
    public int damage;
    [Range(0f, 5f)]
    public float coolDown;

    public void AttackTarget(GameObject target)
    {
        EventManager.DamageActor(target, damage);
    }

}