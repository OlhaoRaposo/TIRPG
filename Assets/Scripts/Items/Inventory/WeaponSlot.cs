using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : InventorySlot
{
    public override void SetItem(ItemObject item)
    {
        if (itemData.item != null && item.item != null) PlayerInventory.instance.AddItemToInventory(itemData.item);

        base.SetItem(item);
    }
    public override void RightClick()
    {
        if (!itemData.Equals(null))
        {
            switch (itemData.item.weaponType)
            {
                case WeaponType.MELEE: PlayerInventory.instance.DropMeleeWeapon(itemData.item); break;
                case WeaponType.RANGED: PlayerInventory.instance.DropRangedWeapon(itemData.item); break;
            }
        }
    }
    public override void LeftClick(){}
}
