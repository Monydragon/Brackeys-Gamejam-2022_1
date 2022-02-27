using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heart Container", menuName = "DLS/Heart Container")]
public class HeartContainerObject : ItemObject
{
    [Range(1, 10)]
    public int healthIncreaseAmount;

    public override void Use(GameObject _obj)
    {
        EventManager.IncreaseHeartContainer(this, _obj);
    }
}