using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemObject
{
    public ItemData item;
    public int amount;

    public ItemObject(ItemData i, int a)
        {
            item = i;
            amount = a;
        } 

}
public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    [SerializeField] InventorySlot[] slots;
    [SerializeField] WeaponSlot meleeWeaponSlot;
    [SerializeField] WeaponSlot rangedWeaponSlot;
    [SerializeField] ConsumableSlot consumableSlot;
    [SerializeField] ConsumableSlot throwableSlot;

    //[SerializeField] List<ItemData> items = new List<ItemData>();
    [SerializeField] List<ItemObject> items = new List<ItemObject>();


    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        EquipStartItems();
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
    public void EquipStartItems()
    {
        InitialEquipRangedWeapon(items[0]);
        InitialEquipMeleeWeapon(items[0]);
        InitialEquipThrowable(items[0]);
        InitialEquipConsumable(items[0]);
    }
    public void UsedConsumable()
    {
        if (consumableSlot.GetItemObject().amount <= 1)
        {
            consumableSlot.SetItem();
        }
        else
        {
            consumableSlot.DecreaseCount();
        }
    }
    public void ThrowedItem()
    {
        if (throwableSlot.GetItemObject().amount <= 1)
        {
            throwableSlot.SetItem();
        }
        else
        {
            throwableSlot.DecreaseCount();
        }
    }
    public bool AddItemToInventory(ItemData itemData)
    {
        //Checa se ja possui o item no inventario
        if (LookForItem(itemData, ref items, true))
        {
            if (!itemData.isStackable)
            {
                if (!TryToAddItemToList(itemData)) return false;
            }
        }
        else
        {
            if (!TryToAddItemToList(itemData)) return false;
        }
        return true;
    }
    public bool AddItemToInventoryInMenu(ItemData itemData)
    {
        //Checa se ja possui o item no inventario
        if (LookForItem(itemData, ref items, true))
        {
            //Checa se o item é stackavel
            if (itemData.isStackable)
            {
                //Adiciona uma unidade ao slot
                FindItemSlot(itemData).AddToCount();
            }
            else //Caso nao seja stackavel
            {
                if (!TryToAddItemToList(itemData)) return false;
            }
        }
        else
        {
            if (!TryToAddItemToList(itemData)) return false;
        }

        SortInventory();

        return true;
    }
    public bool AddItemToInventoryInMenu(ItemObject itemData)
    {
        //Checa se ja possui o item no inventario
        if (LookForItem(itemData, ref items, true, itemData.amount))
        {
            //Checa se o item é stackavel
            if (itemData.item.isStackable)
            {
                //Adiciona uma unidade ao slot
                FindItemSlot(itemData.item).AddToCount(itemData.amount);
            }
        }
        else
        {
            if (!TryToAddItemToList(itemData)) return false;
        }

        SortInventory();

        return true;
    }
    bool TryToAddItemToList(ItemData data)
    {
        //Checa se existem slots disponiveis
        if (items.Count == slots.Length)
        {
            Debug.LogWarning("Cant add item to inventory: The inventory is full");
            return false;
        }
        else //Caso possua slots disponíveis
        {
            items.Add(new ItemObject(data, 1));
            return true;
        }
    }
    bool TryToAddItemToList(ItemObject data)
    {
        //Checa se existem slots disponiveis
        if (items.Count == slots.Length)
        {
            Debug.LogWarning("Cant add item to inventory: The inventory is full");
            return false;
        }
        else //Caso possua slots disponíveis
        {
            items.Add(new ItemObject(data.item, data.amount));
            return true;
        }
    }
    public bool RemoveItemFromInventory(ItemData itemData)
    {
        ItemObject ob;
        if (!LookForItem(itemData, out ob))
        {
            Debug.LogWarning("Item is not in the inventory list");
            return false;
        }

        if(ob.amount <= 1)
        {
            items.Remove(ob);
        }
        else
        {
            LookForAndRemoveItemAmount(ob.item, ref items);
        }
        return true;
    }
    public void RemoveItemFromInventory(ItemObject itemData)
    {
        if (!items.Contains(itemData))
        {
            Debug.LogWarning("O item não está presente no inventário");
            return;
        }

        items.Remove(itemData);
    }
    public bool LookForItem(ItemData itemData)
    {
        foreach (ItemObject i in items)
        {
            if (i.item == itemData) return true;
        }
        return false;
    }
    public bool LookForItem(ItemData itemData, out ItemObject o)
    {
        foreach (ItemObject i in items)
        {
            if (i.item == itemData)
            {
                o = i;
                return true;
            }
        }
        o = new ItemObject(null, -1);
        return false;
    }
    public bool LookForItem(ItemData itemData, ref List<ItemObject> objList, bool canIncrementAmount)
    {
        for(int i = 0; i < objList.Count; i++)
        {
            if (objList[i].item == itemData)
            {
                if (canIncrementAmount)
                {
                    ItemObject o = new ItemObject(objList[i].item, objList[i].amount + 1);
                    objList[i] = o;
                }
                return true;
            }
        }
        return false;
    }
    public bool LookForItem(ItemObject itemData, ref List<ItemObject> objList, bool canIncrementAmount, int incrementAmount)
    {
        for (int i = 0; i < objList.Count; i++)
        {
            if (objList[i].item == itemData.item)
            {
                if (canIncrementAmount)
                {
                    ItemObject o = new ItemObject(objList[i].item, objList[i].amount + incrementAmount);
                    objList[i] = o;
                }
                return true;
            }
        }
        return false;
    }
    public void LookForAndRemoveItemAmount(ItemData itemData, ref List<ItemObject> objList)
    {
        for (int i = 0; i < objList.Count; i++)
        {
            if (objList[i].item == itemData)
            {
                ItemObject o = new ItemObject(objList[i].item, objList[i].amount - 1);
                objList[i] = o;

                FindItemSlot(o.item).DecreaseCount();
            }
        }
    }
    InventorySlot FindItemSlot(ItemData data)
    {
        foreach(InventorySlot slot in slots)
        {
            if (slot.GetItem() == data)
            {
                return slot;
            }
        }

        return null;
    }
    public void DropMeleeWeapon(ItemData itemData)
    {
        meleeWeaponSlot.SetItem();
        ItemDropManager.instance.DropItem(itemData, PlayerInteractions.instance.transform.position + PlayerInteractions.instance.transform.forward);

        PlayerMeleeCombat.instance.enabled = false;
        foreach (GameObject playerWeapon in PlayerMovement.instance.allWeapons)
        {
            if (playerWeapon.name == PlayerMeleeCombat.instance.GetMeleeName())
            {
                playerWeapon.SetActive(false);
                break;
            }
        }
    }
    public void DropRangedWeapon(ItemData itemData)
    {
        rangedWeaponSlot.SetItem();
        ItemDropManager.instance.DropItem(itemData, PlayerInteractions.instance.transform.position + PlayerInteractions.instance.transform.forward);
        
        PlayerGun.instance.enabled = false;
        PlayerMovement.instance.playerModel.transform.Find(PlayerGun.instance.GetGunName()).gameObject.SetActive(false);
        foreach (GameObject playerWeapon in PlayerMovement.instance.allWeapons)
        {
            if (playerWeapon.name == PlayerGun.instance.GetGunName())
            {
                playerWeapon.SetActive(false);
                break;
            }
        }
    }
    public void DropItem(ItemData itemData)
    {
        if (RemoveItemFromInventory(itemData))
        {
            SortInventory();
            ItemDropManager.instance.DropItem(itemData, PlayerInteractions.instance.transform.position + PlayerInteractions.instance.transform.forward);
        }
    }
    void SortInventory()
    {
        ResetItemSlots();

        int itemIndex = 0;
        foreach (InventorySlot slot in slots)
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
        foreach (InventorySlot slot in slots)
        {
            slot.SetItem();
        }
    }
    public void EquipMeleeWeapon(ItemObject weaponData)
    {
        RemoveItemFromInventory(weaponData.item);
        meleeWeaponSlot.SetItem(weaponData);

        SortInventory();

        UIManager.instance.UpdateMeleeWeaponDescription(weaponData.item.meleeBase.damage.ToString(), weaponData.item.meleeBase.damageElement);

        PlayerMeleeCombat.instance?.SetNewMeleeWeapon(weaponData.item.meleeBase);

        foreach (GameObject playerWeapon in PlayerMovement.instance.allWeapons)
        {
            if (playerWeapon.name == PlayerMeleeCombat.instance.GetMeleeName() && PlayerMovement.instance.isRanged == false)
            {
                playerWeapon.SetActive(true);
                PlayerMeleeCombat.instance.enabled = true;
            }
            else
            {
                if (playerWeapon.name != PlayerGun.instance.GetGunName())
                {
                    playerWeapon.SetActive(false);
                }
            }
        }
    }
    public void EquipRangedWeapon(ItemObject weaponData)
    {
        RemoveItemFromInventory(weaponData.item);
        rangedWeaponSlot.SetItem(weaponData);

        SortInventory();

        UIManager.instance.UpdateRangedWeaponDescription(weaponData.item.gunBase.damage.ToString(), weaponData.item.gunBase.bulletElement);

        PlayerGun.instance?.SetNewGunWeapon(weaponData.item.gunBase);

        foreach (GameObject playerWeapon in PlayerMovement.instance.allWeapons)
        {
            if (playerWeapon.name == PlayerGun.instance.GetGunName() && PlayerMovement.instance.isRanged == true)
            {
                playerWeapon.SetActive(true);
                PlayerGun.instance.enabled = true;
            }
            else
            {
                if (playerWeapon.name != PlayerMeleeCombat.instance.GetMeleeName())
                {
                    playerWeapon.SetActive(false);
                }
            }
        }
    }
    public void EquipConsumable(ItemObject consumableData)
    {
        RemoveItemFromInventory(consumableData);
        consumableSlot.SetItem(consumableData);

        SortInventory();
    }
    public void EquipThrowable(ItemObject throwableData)
    {
        RemoveItemFromInventory(throwableData);
        throwableSlot.SetItem(throwableData);

        SortInventory();
    }
    public void InitialEquipMeleeWeapon(ItemObject weaponData)
    {
        RemoveItemFromInventory(weaponData.item);
        meleeWeaponSlot.SetItem(weaponData);

        UIManager.instance.UpdateMeleeWeaponDescription(weaponData.item.meleeBase.damage.ToString(), weaponData.item.meleeBase.damageElement);

        PlayerMeleeCombat.instance?.SetNewMeleeWeapon(weaponData.item.meleeBase);

        foreach (GameObject playerWeapon in PlayerMovement.instance.allWeapons)
        {
            if (playerWeapon.name == PlayerMeleeCombat.instance.GetMeleeName() && PlayerMovement.instance.isRanged == false)
            {
                playerWeapon.SetActive(true);
                PlayerMeleeCombat.instance.enabled = true;
            }
            else
            {
                if (playerWeapon.name != PlayerGun.instance.GetGunName())
                {
                    playerWeapon.SetActive(false);
                }
            }
        }
    }
    public void InitialEquipRangedWeapon(ItemObject weaponData)
    {
        RemoveItemFromInventory(weaponData.item);
        rangedWeaponSlot.SetItem(weaponData);

        UIManager.instance.UpdateRangedWeaponDescription(weaponData.item.gunBase.damage.ToString(), weaponData.item.gunBase.bulletElement);

        PlayerGun.instance?.SetNewGunWeapon(weaponData.item.gunBase);

        foreach (GameObject playerWeapon in PlayerMovement.instance.allWeapons)
        {
            if (playerWeapon.name == PlayerGun.instance.GetGunName() && PlayerMovement.instance.isRanged == true)
            {
                playerWeapon.SetActive(true);
                PlayerGun.instance.enabled = true;
            }
            else
            {
                if (playerWeapon.name != PlayerMeleeCombat.instance.GetMeleeName())
                {
                    playerWeapon.SetActive(false);
                }
            }
        }
    }
    public void InitialEquipConsumable(ItemObject consumableData)
    {
        RemoveItemFromInventory(consumableData);
        consumableSlot.SetItem(consumableData);
    }
    public void InitialEquipThrowable(ItemObject throwableData)
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
    public List<ItemObject> GetInventory()
    {
        List<ItemObject> ret = new List<ItemObject>();

        /*
        if (meleeWeaponSlot.GetItem() != null) ret.Add(meleeWeaponSlot.GetItem());
        if (rangedWeaponSlot.GetItem() != null) ret.Add(rangedWeaponSlot.GetItem());
        if (consumableSlot.GetItem() != null) ret.Add(consumableSlot.GetItem());
        if (throwableSlot.GetItem() != null) ret.Add(throwableSlot.GetItem());
        */

        ret.AddRange(items);

        return ret;
    }
}