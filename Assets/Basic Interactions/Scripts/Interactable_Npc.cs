using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Npc : MonoBehaviour, IInteractable
{
    [SerializeField] bool givesQuest = false;

    /// Atributos a adicionar:
    /// 
    /// Referência da quest
    /// 

    public void Interact(PlayerInteractions player)
    {
        //Iniciar diálogo
        Debug.Log("Iniciando diálogo de " + player.name + " com " + name);
        
        
    }
}
