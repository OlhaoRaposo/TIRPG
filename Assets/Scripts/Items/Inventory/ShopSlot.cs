using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSlot : InventorySlot
{
    [SerializeField] SlotType slotType;
    enum SlotType
    {
        BUY,
        SELL
    }
    public override void LeftClick()
    {
        switch (slotType)
        {
            case SlotType.BUY:
                if (!itemData.Equals(null))
                {
                    //Debug.Log("Comprar " + itemData.name);
                    UIManager.instance.GetCurrentMerchant().BuyItem(itemData.item);
                }
                break;
            case SlotType.SELL:
                if (!itemData.Equals(null))
                {
                    //Debug.Log("Vender " + itemData.name);
                    UIManager.instance.GetCurrentMerchant().SellItem(itemData.item);
                }
                break;
        }
    }
    public override void RightClick(){}
}
