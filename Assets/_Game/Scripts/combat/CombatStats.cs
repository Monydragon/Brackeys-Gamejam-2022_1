using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status", menuName = "Stats/Combat")]
public class CombatStats : ScriptableObject
{
    [Range(0.5f, 5f)]
    public float attackSpeed;
    [Range(1, 5)]
    public int strength;
    [Range(0.5f, 10.0f)]
    public float knockback;
} 