using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Item types")]
    public ItemType itemType;
    public WeaponType weaponType;
    public PlayerMeleeBase meleeBase;
    public PlayerGunBase gunBase;
    [Space]
    public GameObject prefab;
    public bool isStackable;
    public int value;
    public float weight;
    public Sprite sprite;

    public float healingAmount;
}
public enum ItemType
{
    NONE,
    WEAPON,
    CONSUMABLE,
    THROWABLE,
    AMMO
}
public enum WeaponType
{
    NONE,
    MELEE,
    RANGED
}
