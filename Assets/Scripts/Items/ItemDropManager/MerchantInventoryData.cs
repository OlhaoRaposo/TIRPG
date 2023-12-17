using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Merchant Inventory Data", menuName = "Merchant Inventory Data")]
public class MerchantInventoryData : ScriptableObject
{
    public string merchantName;
    public LoyaltySystem.InfluentialSide influentialSide;

    public ItemData[] weapons;
    public int minWeaponAmount;
    public int maxWeaponAmount;

    public ItemData[] consumables;
    public int minConsumableAmount;
    public int maxConsumableAmount;

    public ItemData[] throwables;
    public int minThrowableAmount;
    public int maxThrowableAmount;
}
