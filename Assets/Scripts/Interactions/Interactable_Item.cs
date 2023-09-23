using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Item : MonoBehaviour, IInteractable
{
    /// Atributos a adicionar:
    /// 
    /// Informa��es do item
    /// 


    public void Interact(PlayerInteractions player)
    {
        player.TakeItem(gameObject);

        Destroy(gameObject);
    }
}
