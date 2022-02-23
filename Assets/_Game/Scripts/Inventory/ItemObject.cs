using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "DLS/Item")]
public class ItemObject : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite icon;

    public virtual void Use(GameObject obj)
    {
        EventManager.ItemUse(this, obj);
    }
}
