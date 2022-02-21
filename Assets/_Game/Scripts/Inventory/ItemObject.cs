using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "DLS/Item")]
public class ItemObject : ScriptableObject
{
    public string name;
    public string description;
    public Sprite icon;

    public void Use(GameObject obj)
    {
        EventManager.ItemUse(this, obj);
    }
}
