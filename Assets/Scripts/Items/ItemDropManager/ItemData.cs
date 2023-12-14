using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    public GameObject prefab;
    public int value;
    public float weight;
    public Sprite sprite;

    public ItemType itemType;
    public WeaponType weaponType;
}
public enum ItemType
{
    WEAPON,
    CONSUMABLE,
    THROWABLE
}
public enum WeaponType
{
    NONE,
    MELEE,
    RANGED
}
