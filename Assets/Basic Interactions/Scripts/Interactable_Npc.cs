using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Npc : MonoBehaviour, IInteractable
{
    [SerializeField] bool givesQuest = false;

    /// Atributos a adicionar:
    /// 
    /// Refer�ncia da quest
    /// 

    public void Interact(PlayerInteractions player)
    {
        //Iniciar di�logo
        Debug.Log("Iniciando di�logo de " + player.name + " com " + name);
        
        
    }
}
