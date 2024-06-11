using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_FastTravel : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInteractions player)
    {
        UIManager.instance.ToggleFastTravelMenu();
    }
}
