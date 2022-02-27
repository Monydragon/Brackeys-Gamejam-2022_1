using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public int minimumItemDrop;
    public int maximumItemDrop;
    public Loot[] loot;


    private void OnEnable()
    {
        EventManager.onObjectDied += Drop;
    }
    private void OnDisable()
    {
        EventManager.onObjectDied -= Drop;
    }
    void Drop(GameObject _target)
    {
        if (_target == this.gameObject)
        {
            int dropedItem = Random.Range(minimumItemDrop, maximumItemDrop + 1);
            for (int i = 0; i < dropedItem; i++)
            {
                foreach (Loot _loot in loot)
                {
                    if (_loot.alwaysDrop || Random.Range(0, _loot.rarenes + 1) == 0)
                    {
                        int amount = Random.Range(_loot.minimumDrop, _loot.maximumDrop + 1);
                        if (amount > dropedItem)
                        {
                            amount = dropedItem;
                        }
                        if (amount > 0 && !_loot.alreadyDroped)
                        {
                            _loot.alreadyDroped = true;
                            for (int j = 0; j < amount; j++)
                                ItemPickup.newItem(_loot.item, 1, transform.position
                                + new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
                        }
                        i += amount;
                    }
                    if (!(i < dropedItem))
                        break;
                }
            }
        }
    }
}
/* ↑ the pyramid of ugly code :P*/

// this one is the loot element
[System.Serializable]
public class Loot
{
    [HideInInspector]
    public bool alreadyDroped = false;
    public ItemObject item;
    public int rarenes;
    public bool alwaysDrop;
    [Header("if its actually droped")]
    public int minimumDrop = 1;
    public int maximumDrop = 1;
}