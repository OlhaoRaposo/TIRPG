using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : InventorySlot
{
    public override void SetItem(ItemData item)
    {
        base.SetItem(item);
    }
    public override void RightClick()
    {
        if (itemData != null)
        {
            switch (itemData.weaponType)
            {
                case WeaponType.MELEE: PlayerInventory.instance.DropMeleeWeapon(itemData); break;
                case WeaponType.RANGED: PlayerInventory.instance.DropRangedWeapon(itemData); break;
            }
        }
    }
    public override void LeftClick(){}
}
