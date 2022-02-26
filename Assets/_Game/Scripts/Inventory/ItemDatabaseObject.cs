using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "DLS/Item Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] items;

    public Dictionary<ItemObject, string> GetName = new Dictionary<ItemObject, string>();
    public Dictionary<string, ItemObject> GetItem = new Dictionary<string, ItemObject>();

    public void OnAfterDeserialize()
    {
        GetName = new Dictionary<ItemObject, string>();
        GetItem = new Dictionary<string, ItemObject>();
        for (int i = 0; i < items.Length; i++)
        {
            if(items[i] != null)
            {
                GetName.Add(items[i], items[i].name);
                GetItem.Add(items[i].name, items[i]);
            }
        }
    }

    public void OnBeforeSerialize()
    {

    }

    private void OnEnable()
    {
        GetName = new Dictionary<ItemObject, string>();
        GetItem = new Dictionary<string, ItemObject>();
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                GetName.Add(items[i], items[i].name);
                GetItem.Add(items[i].name, items[i]);
            }
        }
    }
}
