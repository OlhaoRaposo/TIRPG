using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    [SerializeField] List<InventorySlot> slots;
    [SerializeField] WeaponSlot meleeWeaponSlot;
    [SerializeField] WeaponSlot rangedWeaponSlot;
    List<ItemData> items = new List<ItemData>();

    [SerializeField] GameObject inventoryPanel;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //As próximas linhas servem somente para inicializar os slots (vou tirar isso dps)
        ToggleInventoryPanel();
        Invoke("ToggleInventoryPanel", .1f);
    }
    void Update()
    {
        if (Input.GetKeyDown(InputController.instance.inventory))
        {
            ToggleInventoryPanel();
        }
    }
    public void ToggleInventoryPanel()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
    public bool AddItemToInventory(ItemData itemData)
    {
        //Checar se existem slots disponiveis
        if (items.Count == slots.Count)
        {
            Debug.Log("Cant add item to inventory: The inventory is full");
            return false;
        } 

        items.Add(itemData);
        SortInventory();
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
        SortInventory();
        return true;
    }
    public void DropItem(ItemData itemData)
    {
        if (RemoveItemFromInventory(itemData))
        {
            GameObject droppedItem = Instantiate(itemData.prefab, transform.position + Vector3.up, Quaternion.identity);
            ItemDropManager.instance.SetItemParent(droppedItem.transform);
        }
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
