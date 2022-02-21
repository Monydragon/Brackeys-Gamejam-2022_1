using UnityEngine;
using Fungus;

[CommandInfo("DLS",
             "Item Remove",
             "Removes an Item from an Inventory.")]
[AddComponentMenu("")]
public class ItemRemove : Command
{
    [SerializeField] protected InventoryObject inventorySource;
    [SerializeField] protected string itemName;
    [SerializeField] protected int itemAmount;
    public override void OnEnter()
    {
        if(inventorySource != null)
        {
            var item = inventorySource.GetItem(itemName);
            if(item != null)
            {
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