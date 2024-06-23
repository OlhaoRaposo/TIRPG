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

    [Space(10f)]
    [Header("Shared properties")]
    public GameObject prefab;
    public bool isStackable;
    public int value;
    public float weight;
    public Sprite sprite;

    [Space(20f)]
    [Header("Type specific properties")]

    [Space(5f)]
    [Header("Healing properties")]
    public float healingAmount;

    [Space(5f)]
    [Header("Buff properties")]
    public BuffType buffType;
    public float buffAttributeMultiplier;
    public float buffDuration;
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
public enum BuffType
{
    NONE,
    STRENGTH,
    AGILITY,
    ENDURANCE,
    INTELLIGENCE,
    STAMINA_REGENERATION
}
