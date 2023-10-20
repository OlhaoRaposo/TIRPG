using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Npc : MonoBehaviour, IInteractable
{
    [Header("Variables")]
    [SerializeField] private string dialogueCharacter;
    [SerializeField] private int dialogueIndex;
    [SerializeField] bool givesQuest = false;

    /// Atributos a adicionar:
    /// 
    [Header("References")]
    [SerializeField] private QuestType givenQuest;
    /// 

    public void Interact(PlayerInteractions player)
    {
        if (givesQuest == true)
        {
            QuestController.instance.ActivateNextQuest(givenQuest);
        }
    }
}
