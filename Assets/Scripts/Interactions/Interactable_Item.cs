using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Item : MonoBehaviour, IInteractable
{
    [SerializeField] ItemData itemData;
    [SerializeField] QuestType.TypesOfCollectibles typesOf;

    public void Interact(PlayerInteractions player)
    {
        //Entra no if somente se houver espaço disponível no inventário
        if (player.TakeItem(itemData))
        {
            QuestController.instance?.CollectedItems(typesOf);
            InteractTooltip.instance?.ToggleTooltip(null);

            Destroy(gameObject);
        }
    }
}
