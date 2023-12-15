using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (MerchantInventory))]
public class Interactable_Merchant : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInteractions player)
    {
        UIManager.instance.ToggleShopPanel(GetComponent<MerchantInventory>());
    }   
}
