using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    [SerializeField] InventorySlot[] slots;
    [SerializeField] WeaponSlot meleeWeaponSlot;
    [SerializeField] WeaponSlot rangedWeaponSlot;
    [SerializeField] ConsumableSlot consumableSlot;
    [SerializeField] ConsumableSlot throwableSlot;

    [SerializeField] List<ItemData> items = new List<ItemData>();

    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        EquipRangedWeapon(items[0]);
        EquipMeleeWeapon(items[0]);
        EquipThrowable(items[0]);
        EquipConsumable(items[0]);
    }
    void Update()
    {
        if (SceneController.instance.GetIsInMainMenu()) return;

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
            GameObject droppedItem = Instantiate(itemData.prefab, PlayerInteractions.instance.transform.position + (Vector3.up + PlayerInteractions.instance.transform.forward), Quaternion.identity);
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
        GameObject droppedItem = Instantiate(itemData.prefab, PlayerInteractions.instance.transform.position + (Vector3.up + PlayerInteractions.instance.transform.forward), Quaternion.identity);
        PlayerMeleeCombat.instance.gameObject.SetActive(false);
        //ItemDropManager.instance.SetItemParent(droppedItem.transform);
    }
    public void DropRangedWeapon(ItemData itemData)
    {
        rangedWeaponSlot.SetItem(null);
        GameObject droppedItem = Instantiate(itemData.prefab, PlayerInteractions.instance.transform.position + (Vector3.up + PlayerInteractions.instance.transform.forward), Quaternion.identity);
        PlayerGun.instance.gameObject.SetActive(false);
        //ItemDropManager.instance.SetItemParent(droppedItem.transform);

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
        PlayerMeleeCombat.instance?.SetNewMeleeWeapon(weaponData.meleeBase);
    }
    public void EquipRangedWeapon(ItemData weaponData)
    {
        RemoveItemFromInventory(weaponData);
        rangedWeaponSlot.SetItem(weaponData);
        PlayerGun.instance?.SetNewGunWeapon(weaponData.gunBase);
    }
    public void EquipConsumable(ItemData consumableData)
    {
        RemoveItemFromInventory(consumableData);
        consumableSlot.SetItem(consumableData);
    }
    public void EquipThrowable(ItemData throwableData)
    {
        RemoveItemFromInventory(throwableData);
        throwableSlot.SetItem(throwableData);
    }
    public ItemData GetMelee()
    {
        return meleeWeaponSlot.GetItem();
    }
    public ItemData GetRanged()
    {
        return rangedWeaponSlot.GetItem();
    }
    public ItemData GetThrowable()
    {
        return throwableSlot.GetItem();
    }
    public ItemData GetConsumable()
    {
        return consumableSlot.GetItem();
    }
    public List<ItemData> GetInventory()
    {
        List<ItemData> ret = new List<ItemData>();
        /*if (meleeWeaponSlot.GetItem() != null) ret.Add(meleeWeaponSlot.GetItem());
        if (rangedWeaponSlot.GetItem() != null) ret.Add(rangedWeaponSlot.GetItem());
        if (consumableSlot.GetItem() != null) ret.Add(consumableSlot.GetItem());
        if (throwableSlot.GetItem() != null) ret.Add(throwableSlot.GetItem());*/

        ret.AddRange(items);

        return ret;
    }
}
