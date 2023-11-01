using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Item : MonoBehaviour, IInteractable
{
    /// Atributos a adicionar:
    /// 
    /// Informa��es do item
    /// 
    [SerializeField] QuestType.TypesOfCollectibles typesOf;

    public void Interact(PlayerInteractions player)
    {
        player.TakeItem(gameObject);
        QuestController.instance.CollectedItems(typesOf);
        InteractTooltip.instance.ToggleTooltip(null);

        Destroy(gameObject);
    }
}
