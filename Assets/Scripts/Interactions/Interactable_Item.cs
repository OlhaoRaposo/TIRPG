using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Item : MonoBehaviour, IInteractable
{
    [SerializeField] ItemData[] itemData;
    
    //[SerializeField] QuestType.TypesOfCollectibles typesOf;

    public void Interact(PlayerInteractions player)
    {
        if (itemData.Length < 1)
        {
            Debug.LogError("No ItemData assigned to the object");
            return;
        }

        string itemsPicked = "";
        bool hasTakenItem = false;
        foreach(ItemData item in itemData)
        {
            //Entra no if somente se houver espa�o dispon�vel no invent�rio
            hasTakenItem = player.TakeItem(item);
            if (hasTakenItem)
            {
                if (itemsPicked.Length > 1) itemsPicked += ", ";

                itemsPicked += item.name;

                //QuestController.instance?.CollectedItems(typesOf);
            }
        }
        if (itemsPicked.Length > 1) UIManager.instance.ShowTextFeedback($"Picked up: {itemsPicked}");

        if (hasTakenItem)
        {
            InteractTooltip.instance?.ToggleTooltip(null);
            Destroy(gameObject);
        }
    }
    public void SetItemData(ItemData[] data)
    {
        itemData = data;
    }
    public void SetItemData(ItemData data)
    {
        ItemData[] tempArray = new ItemData[] { data };
        itemData = tempArray;
    }
}
