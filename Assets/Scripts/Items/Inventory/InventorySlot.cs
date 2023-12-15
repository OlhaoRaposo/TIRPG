using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    protected Image itemImage;
    protected ItemData itemData;

    void OnEnable()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
    }
    public virtual void SetItem(ItemData item)
    {
        itemData = item;

        if (item != null){
            Color color = itemImage.color;
            color.a = 1f;
            itemImage.color = color;
            itemImage.sprite = item.sprite;
            itemImage.preserveAspect = true;
        }else{
            Color color = itemImage.color;
            color.a = 0f;
            itemImage.color = color;
            itemImage.sprite = null;
        }
    }
    public ItemData GetItem()
    {
        return itemData;
    }
    public virtual void LeftClick()
    {
        if (itemData == null) return;
        
        switch (itemData.itemType)
        {
            case ItemType.WEAPON:
                switch(itemData.weaponType)
                {   
                    case WeaponType.MELEE: PlayerInventory.instance.EquipMeleeWeapon(itemData); break;
                    case WeaponType.RANGED: PlayerInventory.instance.EquipRangedWeapon(itemData); break;
                }   
                break;
            case ItemType.CONSUMABLE:
                PlayerInventory.instance.EquipConsumable(itemData);
                break;
            case ItemType.THROWABLE:
                PlayerInventory.instance.EquipThrowable(itemData);
                break;
        }
        
    }
    public virtual void RightClick()
    {
        if (itemData != null)
        {
            PlayerInventory.instance.DropItem(itemData);
        }
    }

    //Detecta se um clique ocorreu sobre o slot
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                LeftClick();
                break;
            case PointerEventData.InputButton.Right:
                RightClick();
                break;
        }
    }
}
