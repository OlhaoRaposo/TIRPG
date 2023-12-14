using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    [SerializeField] InventorySlot[] slots;
    [SerializeField] WeaponSlot meleeWeaponSlot;
    [SerializeField] WeaponSlot rangedWeaponSlot;
    List<ItemData> items = new List<ItemData>();

    void Awake()
    {
        instance = this;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(InputController.instance.inventory))
        {
            UIManager.instance?.ToggleInGameMenus();
            SortInventory();
        }
    }
    public bool AddItemToInventory(ItemData itemData)
    {
        //Checar se existem slots disponiveis
        if (items.Count == slots.Length)
        {
            Debug.Log("Cant add item to inventory: The inventory is full");
            return false;
        } 

        items.Add(itemData);
        return true;
    }
    public bool RemoveItemFromInventory(ItemData itemData)
    {
        if (!items.Contains(itemData))
        {
            Debug.Log("Item is not in the inventory list");
            return false;
        }

        items.Remove(itemData);
        return true;
    }
    public GameObject DropItem(ItemData itemData)
    {
        if (RemoveItemFromInventory(itemData))
        {
            SortInventory();
            GameObject droppedItem = Instantiate(itemData.prefab, transform.position + Vector3.up, Quaternion.identity);
            ItemDropManager.instance.SetItemParent(droppedItem.transform);
            return droppedItem;
        }
        return null;
    }
    public bool LookForItem(ItemData itemData)
    {
        foreach(ItemData item in items)
        {
            if (item == itemData)
            {
                return true;
            }
        }
        return false;
    }
    public void DropMeleeWeapon(ItemData itemData)
    {
        meleeWeaponSlot.SetItem(null);
        GameObject droppedItem = Instantiate(itemData.prefab, transform.position + Vector3.up, Quaternion.identity);
        ItemDropManager.instance.SetItemParent(droppedItem.transform);
    }
    public void DropRangedWeapon(ItemData itemData)
    {
        rangedWeaponSlot.SetItem(null);
        GameObject droppedItem = Instantiate(itemData.prefab, transform.position + Vector3.up, Quaternion.identity);
        ItemDropManager.instance.SetItemParent(droppedItem.transform);
    }
    void SortInventory()
    {
        ResetItemSlots();

        int itemIndex = 0;
        foreach(InventorySlot slot in slots)
        {
            if (itemIndex + 1 > items.Count)
            {
                break;
            }
            slot.SetItem(items[itemIndex]);
            itemIndex++;
        }
    }
    void ResetItemSlots()
    {
        foreach(InventorySlot slot in slots)
        {
            slot.SetItem(null);
        }
    }
    public List<ItemData> GetInventory()
    {
        List<ItemData> ret = new List<ItemData>();
        if (meleeWeaponSlot.GetItem() != null) ret.Add(meleeWeaponSlot.GetItem());
        if (rangedWeaponSlot.GetItem() != null) ret.Add(rangedWeaponSlot.GetItem());

        ret.AddRange(items);

        return ret;
    }
    public void EquipMeleeWeapon(ItemData weaponData)
    {
        RemoveItemFromInventory(weaponData);
        meleeWeaponSlot.SetItem(weaponData);
    }
    public void EquipRangedWeapon(ItemData weaponData)
    {
        RemoveItemFromInventory(weaponData);
        rangedWeaponSlot.SetItem(weaponData);
    }
}
