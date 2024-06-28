using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Merchant Inventory Data", menuName = "Merchant Inventory Data")]
public class MerchantInventoryData : ScriptableObject
{
    public string merchantName;
    public LoyaltySystem.InfluentialSide influentialSide;

    [Header("Weapons")]
    public ItemData[] weapons;
    public int minWeaponAmount;
    public int maxWeaponAmount;

    [Header("Ammo")]
    public ItemData[] ammo;
    public int minAmmoAmount;
    public int maxAmmoAmount;

    [Header("Consumables")]
    public ItemData[] consumables;
    public int minConsumableAmount;
    public int maxConsumableAmount;

    [Header("Throwables")]
    public ItemData[] throwables;
    public int minThrowableAmount;
    public int maxThrowableAmount;
}
