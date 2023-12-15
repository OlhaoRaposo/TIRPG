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

    public bool isWeapon = false;
    public WeaponType weaponType;
}
public enum WeaponType
{
    NONE,
    MELEE,
    RANGED
}
