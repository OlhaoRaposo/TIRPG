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
    void Start()
    {
        if (droppedItemsParent == null) droppedItemsParent = transform;
    }
    public void DropItem(ItemDropInfo[] dropInfo, Vector3 instantiatePosition)
    {
        if (dropInfo == null) return;

        foreach(ItemDropInfo item in dropInfo)
        {
            if (Random.Range(0f, 1f) < item.dropRate)
            {
                Instantiate(item.data.prefab, instantiatePosition + Vector3.up, Quaternion.identity, droppedItemsParent);
            }
        }
    }
    public void SetItemParent(Transform item)
    {
        item.SetParent(droppedItemsParent);
    }
}

[System.Serializable]
public struct ItemDropInfo
{
    public ItemData data;
    public float dropRate;
    //public int amount;
}