using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    public static ItemDropManager instance;

    [SerializeField] Transform droppedItemsParent;
    void Awake()
    {
        instance = this;
    }

    public void DropItem(ItemDropInfo[] dropInfo, Vector3 instantiatePosition)
    {
        foreach(ItemDropInfo item in dropInfo)
        {
            if (Random.Range(0f, 1f) < item.dropRate)
            {
                Instantiate(item.itemPrefab, instantiatePosition, Quaternion.identity, droppedItemsParent);
            }
        }
    }
}

[System.Serializable]
public struct ItemDropInfo
{
    public GameObject itemPrefab;
    public float dropRate;
}