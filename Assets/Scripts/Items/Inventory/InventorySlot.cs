using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    protected Image itemImage;
    protected ItemData itemData;
    void Awake()
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
    public virtual void EquipItem()
    {
        if (itemData != null && itemData.isWeapon)
        {
            switch(itemData.weaponType)
            {
                case WeaponType.MELEE: PlayerInventory.instance.EquipMeleeWeapon(itemData); break;
                case WeaponType.RANGED: PlayerInventory.instance.EquipRangedWeapon(itemData); break;
            }
        }
    }
    public virtual void DropItem()
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
                EquipItem();
                break;
            case PointerEventData.InputButton.Right:
                DropItem();
                break;
        }
    }
}
