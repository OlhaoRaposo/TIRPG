using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Treasure Chest", menuName = "Treasure Chest/New Treasure Chest")]

public class TreasureChest : ScriptableObject
{
    public GameObject prefab;
    public int level;
    public int amountRewards;
    public List<ItemData> possibleRewards = new List<ItemData>();
}
