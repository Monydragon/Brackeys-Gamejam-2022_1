using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "DLS/Food")]
public class FoodObject : ItemObject
{
    [Range(1, 20)]
    public int healAmount;

    public override void Use(GameObject _obj)
    {
        EventManager.FoodEat(this, _obj);
    }
}
