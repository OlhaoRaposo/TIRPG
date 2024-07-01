using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableSlot : InventorySlot
{
    public override void SetItem(ItemObject item)
    {
        if (itemData.item != null && item.item != null) PlayerInventory.instance.AddItemToInventoryInMenu(itemData);

        base.SetItem(item);
    }
    public override void RightClick()
    {
        if (itemData.item != null)
        {
            //PlayerInventory.instance.DropConsumable(itemData.item);
        }
    }
    public override void LeftClick() { }
}
