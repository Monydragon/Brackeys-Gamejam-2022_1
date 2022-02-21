using UnityEngine;
using Fungus;

[CommandInfo("DLS",
             "Item Give From DB",
             "Gives an Item from the DB to another Inventory")]
[AddComponentMenu("")]
public class ItemGiveFromDB : Command
{
    [SerializeField] protected ItemDatabaseObject databaseSource;
    [SerializeField] protected InventoryObject inventoryTarget;
    [SerializeField] protected string itemName;
    [SerializeField] protected int itemAmount;
    public override void OnEnter()
    {
        if(databaseSource != null && inventoryTarget != null)
        {
            var item = databaseSource.GetItem[itemName];
            if(item != null)
            {
                inventoryTarget.AddItem(item, itemAmount);
            }
            else
            {
                Debug.LogWarning($"Item: {itemName} does not exist in inventorySource");
            }

        }
        Continue();
    }
}