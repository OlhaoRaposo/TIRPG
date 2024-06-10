using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class MerchantInventory : MonoBehaviour
{
    //Itens que podem ser vendidos pelo npc
    [SerializeField] MerchantInventoryData inventoryData;

    List<ItemData> weapons = new List<ItemData>();
    List<ItemData> consumables = new List<ItemData>();
    List<ItemData> throwables = new List<ItemData>();

    void Start()
    {
        SetInventory();
    }
    public void BuyItem(ItemData item)
    {
        //Checar condicao de lealdade
        if (CheckEnoughLoyaltyPoints(item.value))            
        {
            //Adicionar item ao inventario
            if (PlayerInventory.instance.AddItemToInventory(item))
            {
                SpendLoyaltyPoints(item.value);
                //Remover o item do inventario do mercador
                RemoveItem(item);
                SortInventory();
            }
        }
    }
    void SpendLoyaltyPoints(int value)
    {
        switch (inventoryData.influentialSide)
        {
            case LoyaltySystem.InfluentialSide.Nature:
                LoyaltySystem.instance.RemovePointsInfluenceNature(value);
                break;
            case LoyaltySystem.InfluentialSide.City:
                LoyaltySystem.instance.RemovePointsInfluenceCity(value);
                break;
        }
        UIManager.instance.UpdateShopInfluenceInfo();      
    }
    bool CheckEnoughLoyaltyPoints(int value)
    {
        switch (inventoryData.influentialSide)
        {
            case LoyaltySystem.InfluentialSide.Nature:
                if (LoyaltySystem.instance.GetInfluencePointsNature() >= value){
                    return true;
                }else{
                    return false;
                }
            case LoyaltySystem.InfluentialSide.City:
                if (LoyaltySystem.instance.GetInfluencePointsCity() >= value){
                    return true;
                }else{
                    return false;
                }
        }
        return true;
    }
    public void SellItem(ItemData item)
    {
        //Checar se tem espaço no inventario do mercador
        if (UIManager.instance.GetShopSlotsBuy().Length == GetAllItems().Count) return;

        //Adicionar pontos de influencia
        switch (inventoryData.influentialSide)
        {
            case LoyaltySystem.InfluentialSide.Nature:
                LoyaltySystem.instance.AddPointsInfluenceNature(item.value);
                break;
            case LoyaltySystem.InfluentialSide.City:
                LoyaltySystem.instance.AddPointsInfluenceCity(item.value);
                break;
        }

        //Remover item do inventario do player
        PlayerInventory.instance.RemoveItemFromInventory(item);

        //Adicionar item no inventario do mercador
        AddItem(item);
        SortInventory();
        UIManager.instance.UpdateShopInfluenceInfo();
    }
    void AddItem(ItemData item)
    {
        switch (item.itemType)
        {
            case ItemType.WEAPON:
                weapons.Add(item);
                break;
            case ItemType.CONSUMABLE:
                consumables.Add(item);
                break;
            case ItemType.THROWABLE:
                throwables.Add(item);
                break;
        }
    }
    void RemoveItem(ItemData item)
    {
        switch (item.itemType)
        {
            case ItemType.WEAPON:
                if (weapons.Contains(item))
                {
                    weapons.Remove(item);
                }
                else
                {
                    Debug.LogError("O item não está no inventario de armas do npc");
                }
                break;
            case ItemType.CONSUMABLE:
                if (consumables.Contains(item))
                {
                    consumables.Remove(item);
                }
                else
                {
                    Debug.LogError("O item não está no inventario de consumiveis do npc");
                }
                break;
            case ItemType.THROWABLE:
                if (throwables.Contains(item))
                {
                    throwables.Remove(item);
                }
                else
                {
                    Debug.LogError("O item não está no inventario de arremessaveis do npc");
                }
                break;
        }
    }
    void SetInventory()
    {
        SetWeapons();
        SetConsumables();
        SetThrowables();
    }
    void SetWeapons()
    {
        int amount = Random.Range(inventoryData.minWeaponAmount, inventoryData.maxWeaponAmount + 1);

        for (int i = 0; i < amount; i++)
        {
            weapons.Add(inventoryData.weapons[Random.Range(0, inventoryData.weapons.Length)]);
        }
    }
    void SetConsumables()
    {
        int amount = Random.Range(inventoryData.minConsumableAmount, inventoryData.maxConsumableAmount + 1);

        for (int i = 0; i < amount; i++)
        {
            consumables.Add(inventoryData.consumables[Random.Range(0, inventoryData.consumables.Length)]);
        }
    }
    void SetThrowables()
    {
        int amount = Random.Range(inventoryData.minThrowableAmount, inventoryData.maxThrowableAmount + 1);

        for (int i = 0; i < amount; i++)
        {
            throwables.Add(inventoryData.throwables[Random.Range(0, inventoryData.throwables.Length)]);
        }
    }
    void SortInventory()
    {
        ResetSlots();

        SortToSellItems();
        SortToBuyItems();
    }
    void SortToSellItems()
    {
        int itemIndex = 0;
        foreach (ShopSlot slot in UIManager.instance.GetShopSlotsSell())
        {
            if (itemIndex + 1 > PlayerInventory.instance.GetInventory().Count)
            {
                break;
            }
            slot.SetItem(PlayerInventory.instance.GetInventory()[itemIndex]);
            itemIndex++;
        }
    }
    void SortToBuyItems()
    {
        List<ItemData> items = GetAllItems();

        int itemIndex = 0;
        foreach (ShopSlot slot in UIManager.instance.GetShopSlotsBuy())
        {
            if (itemIndex + 1 > items.Count)
            {
                break;
            }
            slot.SetItem(new ItemObject(items[itemIndex], 1));
            itemIndex++;
        }
    }
    List<ItemData> GetAllItems()
    {
        List<ItemData> items = new List<ItemData>();
        items.AddRange(weapons);
        items.AddRange(consumables);
        items.AddRange(throwables);

        return items;
    }
    void ResetSlots()
    {
        foreach (ShopSlot slot in UIManager.instance.GetShopSlotsBuy())
        {
            slot.SetItem();
        }
        foreach (ShopSlot slot in UIManager.instance.GetShopSlotsSell())
        {
            slot.SetItem();
        }
    }
    public void SetShopUI()
    {
        int slotIndex = 0;

        if (weapons.Count > 0)
        {
            foreach (ItemData data in weapons)
            {
                UIManager.instance.GetShopSlotsBuy()[slotIndex].SetItem(new ItemObject(data, 1));
                slotIndex++;
            }
        }

        if (consumables.Count > 0)
        {
            foreach (ItemData data in consumables)
            {
                UIManager.instance.GetShopSlotsBuy()[slotIndex].SetItem(new ItemObject(data, 1));
                slotIndex++;
            }
        }

        if (throwables.Count > 0)
        {
            foreach (ItemData data in throwables)
            {
                UIManager.instance.GetShopSlotsBuy()[slotIndex].SetItem(new ItemObject(data, 1));
                slotIndex++;
            }
        }

        SortInventory();
    }
    public MerchantInventoryData GetInventoryData()
    {
        return inventoryData;
    }
}
