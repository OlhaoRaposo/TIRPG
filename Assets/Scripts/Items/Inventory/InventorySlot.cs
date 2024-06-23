using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected Image itemImage;

    [SerializeField] protected Text itemAmountText;
    int itemAmount = 0;

    protected ItemObject itemData = new ItemObject();

    void OnEnable()
    {
        if (itemImage == null) itemImage = transform.GetChild(0).GetComponent<Image>();

        if (itemAmountText == null) itemAmountText = transform.GetComponentInChildren<Text>();
    }
    public virtual void SetItem(ItemObject itemObj)
    {
        itemData = itemObj;

        SetItemSprite(itemObj);

        SetItemAmount(itemObj);
    }
    public void SetItem()
    {
        itemData.item = null;
        itemData.amount = 0;

        Color color = itemImage.color;
        color.a = 0f;
        itemImage.color = color;
        itemImage.sprite = null;

        itemAmount = 0;
        itemAmountText.text = "";
    }
    void SetItemSprite(ItemObject itemObj)
    {
        if (itemImage == null) return;

        if (itemObj.item != null)
        {
            Color color = itemImage.color;
            color.a = 1f;
            itemImage.color = color;

            //Debug.Log(itemObj.item.name, itemObj.item);
            if (itemObj.item.sprite == null)
            {
                Debug.LogError($"ERRO: O item {itemObj.item.name} está sem um icone atribuído");
            }
            else
            {
                itemImage.sprite = itemObj.item.sprite;
            }

            itemImage.preserveAspect = true;

        }
    }
    void SetItemAmount(ItemObject itemObj)
    {
        if (itemAmountText == null) return;
        
        Debug.Log("amount: " + itemObj.amount, this);

        if (itemObj.amount > 1 && itemObj.item.isStackable)
        {
            itemAmount = itemObj.amount;
            itemAmountText.text = itemAmount.ToString();
        }

    }
    public void AddToCount()
    {
        itemAmount++;
        itemAmountText.text = itemAmount.ToString();
    }
    public void DecreaseCount()
    {
        itemAmount = Mathf.Clamp(itemAmount--, 0, 999);
        itemAmountText.text = itemAmount.ToString();
    }
    public ItemData GetItem()
    {
        return itemData.item;
    }
    public ItemObject GetItemObject()
    {
        return itemData;
    }
    public virtual void LeftClick()
    {
        if (itemData.item == null) return;
        
        switch (itemData.item.itemType)
        {
            case ItemType.WEAPON:
                switch(itemData.item.weaponType)
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
        if (!itemData.Equals(null))
        {
            PlayerInventory.instance.DropItem(itemData.item);
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
