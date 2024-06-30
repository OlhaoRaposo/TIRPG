using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    public static ItemDropManager instance;

    [SerializeField] GameObject orbPrefab;

    [SerializeField] Transform droppedItemsParent;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (droppedItemsParent == null) droppedItemsParent = transform;
    }
    public void DropItem(ItemData data, Vector3 instantiatePosition)
    {
        if (data == null) return;

        GameObject orb = Instantiate(orbPrefab, instantiatePosition + Vector3.up, Quaternion.identity, droppedItemsParent);
        orb.GetComponent<Interactable_Item>().SetItemData(data);

        SetItemParent(droppedItemsParent);
    }
    public void DropItem(ItemDropInfo[] dropInfo, Vector3 instantiatePosition)
    {
        if (dropInfo == null) return;

        List<ItemData> droppedItemsData = new List<ItemData>();

        foreach(ItemDropInfo item in dropInfo)
        {
            if (Random.Range(0f, 1f) < item.dropRate)
            {
                //Instantiate(item.data.prefab, instantiatePosition + Vector3.up, Quaternion.identity, droppedItemsParent);

                droppedItemsData.Add(item.data);
            }
        }
        GameObject orb = Instantiate(orbPrefab, instantiatePosition + Vector3.up, Quaternion.identity, droppedItemsParent);
        orb.GetComponent<Interactable_Item>().SetItemData(droppedItemsData.ToArray());

        SetItemParent(droppedItemsParent);
    }
    public void SetItemParent(Transform item)
    {
        if (item == null)
        {
            item.SetParent(transform);
            return;
        }

        item.SetParent(droppedItemsParent);
    }

    public void DestroyDroppedItems()
    {
        foreach(Transform go in droppedItemsParent.GetComponentsInChildren<Transform>())
        {
            Destroy(go.gameObject);
        }
    }
}

[System.Serializable]
public struct ItemDropInfo
{
    public ItemData data;
    public float dropRate;
    //public int amount;
}