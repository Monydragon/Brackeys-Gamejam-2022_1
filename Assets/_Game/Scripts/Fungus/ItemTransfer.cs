using UnityEngine;
using Fungus;

[CommandInfo("DLS",
             "Item Transfer",
             "Gives an Item from an Inventory to another Inventory")]
[AddComponentMenu("")]
public class ItemTransfer : Command
{
    [SerializeField] protected InventoryObject inventorySource;
    [SerializeField] protected InventoryObject inventoryTarget;
    [SerializeField] protected string itemName;
    [SerializeField] protected int itemAmount;
    public override void OnEnter()
    {
        if(inventorySource != null && inventoryTarget != null)
        {
            var item = inventorySource.GetItem(itemName);
            if(item != null)
            {
                inventoryTarget.AddItem(item, itemAmount);
                inventorySource.RemoveItem(item);
            }
            else
            {
                Debug.LogWarning($"Item: {itemName} does not exist in inventorySource");
            }

        }
        Continue();
    }
}