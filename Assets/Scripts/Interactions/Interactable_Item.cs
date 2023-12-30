using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Item : MonoBehaviour, IInteractable
{
    [SerializeField] ItemData itemData;
    [SerializeField] QuestType.TypesOfCollectibles typesOf;

    public void Interact(PlayerInteractions player)
    {
        //Entra no if somente se houver espa�o dispon�vel no invent�rio
        if (player.TakeItem(itemData))
        {
            UIManager.instance.ShowTextFeedback($"Picked up: {itemData.name}");
            //QuestController.instance?.CollectedItems(typesOf);
            InteractTooltip.instance?.ToggleTooltip(null);

            Destroy(gameObject);
        }
    }
}
