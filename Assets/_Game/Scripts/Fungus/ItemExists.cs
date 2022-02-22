using UnityEngine;
using Fungus;

[CommandInfo("DLS",
             "Item Exists",
             "Check an Inventory if an item exists.")]
[AddComponentMenu("")]
public class ItemExists : Command
{
    [SerializeField] protected InventoryObject inventorySource;
    [SerializeField] protected string itemName;
    [VariableProperty(typeof(BooleanVariable))]
    [SerializeField] protected Variable exists;
    public override void OnEnter()
    {
        if(inventorySource != null)
        {
            exists.Apply(SetOperator.Assign, inventorySource.ItemExists(itemName));
        }
        Continue();
    }
}