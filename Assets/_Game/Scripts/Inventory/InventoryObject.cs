using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Inventory", menuName = "DLS/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath = "PlayerInventory.save";
    public List<InventorySlot> container = new List<InventorySlot>();
    public int currentSize = 0;
    public int maxSize = 8;
    private ItemDatabaseObject database;

    private void OnEnable()
    {
#if UNITY_EDITOR
        database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath<ItemDatabaseObject>("Assets/_Game/Resources/ItemDatabase.asset");
#else
        database = Resources.Load<ItemDatabaseObject>("ItemDatabase");
#endif
    }

    public void AddItem(ItemObject _item, int _amount)
    {
        var foundItem = container.Find(x=> x.item == _item);
        if (foundItem != null)
        {
            foundItem.amount += _amount;
        }
        else
        {
            if(currentSize < maxSize)
            {
                container.Add(new InventorySlot(_item, _amount));
                currentSize++;
            }
        }
        EventManager.InventoryChanged(this);

    }

    public void RemoveItem(ItemObject _item)
    {
        var foundItem = container.Find(x => x.item == _item);
        if(foundItem != null)
        {
            if(foundItem.amount > 1)
            {
                foundItem.amount--;
            }
            else
            {
                container.Remove(foundItem);
                currentSize--;
            }
        }
        EventManager.InventoryChanged(this);
    }

    public ItemObject GetItem(string _name)
    {
        return container.Find(x=> x.item.name == _name).item;
    }

    public InventorySlot GetItemSlot(string _name)
    {
        return container.Find(x => x.item.name == _name);
    }

    public bool ItemExists(string _name)
    {
        return container.Exists(x=> x.item.name == _name);
    }

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < container.Count; i++)
        {
            if(container[i] != null)
            {
                container[i].item = database.GetItem[container[i].item.name];
            }
        }
    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
        EventManager.InventoryChanged(this);
    }

}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;

    public InventorySlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
}
